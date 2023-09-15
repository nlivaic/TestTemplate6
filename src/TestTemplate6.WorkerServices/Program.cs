using System;
using System.Reflection;
using Azure.Monitor.OpenTelemetry.Exporter;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SparkRoseDigital.Infrastructure.Caching;
using SparkRoseDigital.Infrastructure.Logging;
using TestTemplate6.Common.MessageBroker.Middlewares.ErrorLogging;
using TestTemplate6.Common.MessageBroker.Middlewares.Tracing;
using TestTemplate6.Core;
using TestTemplate6.Core.Events;
using TestTemplate6.Data;
using TestTemplate6.WorkerServices.FaultService;
using TestTemplate6.WorkerServices.FooService;
using LoggerExtensions = SparkRoseDigital.Infrastructure.Logging.LoggerExtensions;

namespace TestTemplate6.WorkerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerExtensions.ConfigureSerilogLogger("DOTNET_ENVIRONMENT");

            try
            {
                Log.Information("Starting up TestTemplate6 Worker Services.");
                CreateHostBuilder(args)
                    .Build()
                    .AddW3CTraceContextActivityLogging()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "TestTemplate6 Worker Services failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var hostEnvironment = hostContext.HostingEnvironment;
                    services.AddDbContext<TestTemplate6DbContext>(options =>
                    {
                        var connString = new SqlConnectionStringBuilder(configuration.GetConnectionString("TestTemplate6DbConnection"))
                        {
                            UserID = configuration["DB_USER"],
                            Password = configuration["DB_PASSWORD"]
                        };
                        options.UseSqlServer(connString.ConnectionString);
                        if (hostEnvironment.IsDevelopment())
                        {
                            options.EnableSensitiveDataLogging(true);
                        }
                    });
                    services.AddGenericRepository();
                    services.AddSpecificRepositories();
                    services.AddCoreServices();
                    services.AddSingleton<ICache, Cache>();
                    services.AddMemoryCache();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Program).Assembly);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FooConsumer>();
                        x.AddConsumer<FooFaultConsumer>();
                        x.AddConsumer<FaultConsumer>();
                        x.AddConsumer<FooCommandConsumer>(typeof(FooCommandConsumer.FooCommandConsumerDefinition));

                        if (string.IsNullOrEmpty(configuration.GetConnectionString("MessageBroker")))
                        {
                            x.UsingInMemory();
                        }
                        else
                        {
                            x.UsingAzureServiceBus((ctx, cfg) =>
                            {
                                cfg.Host(configuration.GetConnectionString("MessageBroker"));

                                // Use the below line if you are not going with
                                // SetKebabCaseEndpointNameFormatter() in the publishing project (see API project),
                                // but have rather given the topic a custom name.
                                // cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("foo-topic"));
                                cfg.SubscriptionEndpoint<IFooEvent>("foo-event-subscription-1", e =>
                                {
                                    e.ConfigureConsumer<FooConsumer>(ctx);
                                });

                                // This is here only for show. I have not thought through a proper
                                // error handling strategy.
                                cfg.SubscriptionEndpoint<Fault<IFooEvent>>("foo-event-fault-consumer", e =>
                                {
                                    e.ConfigureConsumer<FooFaultConsumer>(ctx);
                                });

                                // This is here only for show. I have not thought through a proper
                                // error handling strategy.
                                cfg.SubscriptionEndpoint<Fault>("fault-consumer", e =>
                                {
                                    e.ConfigureConsumer<FaultConsumer>(ctx);
                                });
                                cfg.ConfigureEndpoints(ctx);

                                cfg.UseMessageBrokerTracing();
                                cfg.UseExceptionLogger(services);
                            });
                        }
                        x.AddEntityFrameworkOutbox<TestTemplate6DbContext>(o =>
                        {
                            // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                            o.UseSqlServer();

                            // enable the bus outbox
                            o.UseBusOutbox();
                        });
                    });
                    if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
                    {
                        services
                            .AddOpenTelemetry()
                            .WithTracing(tracerProviderBuilder =>
                            {
                                tracerProviderBuilder
                                    .AddSource(WorkerAssemblyInfo.Value.GetName().Name)
                                    .SetResourceBuilder(
                                        ResourceBuilder
                                            .CreateDefault()
                                            .AddService(serviceName: WorkerAssemblyInfo.Value.GetName().Name))
                                    .AddEntityFrameworkCoreInstrumentation()
                                    .AddSqlClientInstrumentation()
                                    .AddSource("MassTransit")
                                    .AddAzureMonitorTraceExporter(o =>
                                    {
                                        o.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
                                    });
                            })//.WithMetrics(meterProviderBuilder =>
                              //{
                              //    meterProviderBuilder
                              //        .SetResourceBuilder(
                              //            ResourceBuilder
                              //                .CreateDefault()
                              //                .AddService(serviceName: "TestTemplate2"))
                              //        .AddAspNetCoreInstrumentation()
                              //        .AddAzureMonitorMetricExporter(o =>
                              //        {
                              //            //o.ConnectionString = "InstrumentationKey=f051d7dd-dbaf-450a-a6f1-9f78bc0f8c91";
                              //            o.ConnectionString = "InstrumentationKey=f051d7dd-dbaf-450a-a6f1-9f78bc0f8c91;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";
                              //        })
                              //        .AddConsoleExporter();
                              //})
                            .StartWithHost();
                    }
                });
    }
}
