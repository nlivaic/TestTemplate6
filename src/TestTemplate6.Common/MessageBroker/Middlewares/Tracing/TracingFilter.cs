using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;

namespace TestTemplate6.Common.MessageBroker.Middlewares.Tracing
{
    public class TracingFilter : IFilter<ConsumeContext>
    {
        public void Probe(ProbeContext context)
        {
        }

        /// <summary>
        /// Reads MT-Activity-Id and creates a new Activity. MT-Activity-Id should be formatted
        /// as per W3C Trace Context recommendations on traceparent header.
        /// This way tracing is enabled across services.
        /// If MT-Activity-Id is not found (or is empty) a brand new Trace Id is created.
        /// </summary>
        /// <param name="context">The context sent through the pipeline.</param>
        /// <param name="next">The next filter in the pipe, must be called or the pipe ends here.</param>
        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            context.Headers.TryGetHeader("MT-Activity-Id", out object traceparentHeader);
            using var activity = new Activity("WorkerServices.Consumer");
            var traceheader = (string)traceparentHeader;
            if (!string.IsNullOrEmpty(traceheader))
            {
                activity.SetParentId((string)traceparentHeader);
            }
            activity.Start();

            // Call next middleware
            await next.Send(context);
            activity.Stop();
        }
    }
}
