using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using TestTemplate6.Data;
using Xunit.Abstractions;

namespace TestTemplate6.Api.Tests.Helpers
{
    public class MsSqlContainer
    {
        private const string _database = "master";
        private const string _username = "sa";
        private const string _password = "yourStrong(!)Password";
        private const int _msSqlPort = 1433;
        private readonly IContainer _mssqlContainer = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU1-ubuntu-20.04")
            .WithPortBinding(_msSqlPort, true)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", _username)
            .WithEnvironment("SQLCMDPASSWORD", _password)
            .WithEnvironment("MSSQL_SA_PASSWORD", _password)
            .WithWaitStrategy(
                Wait.ForUnixContainer().UntilCommandIsCompleted(
                    "/opt/mssql-tools/bin/sqlcmd",
                    "-Q",
                    "SELECT 1;"))
            .Build();
        public string ConnectionString =>
            $"Server={_mssqlContainer.Hostname},{_mssqlContainer.GetMappedPublicPort(_msSqlPort)};" +
            $"Database={_database};" +
            $"User Id={_username};" +
            $"Password={_password};" +
            $"TrustServerCertificate=True";

        public Task InitializeAsync() =>
            _mssqlContainer.StartAsync();

        public Task DisposeAsync() =>
            _mssqlContainer.DisposeAsync().AsTask();
    }
}
