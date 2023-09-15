using System.Threading.Tasks;
using MassTransit;

namespace TestTemplate6.WorkerServices.FaultService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make FooConsumer throw in order to kick error handling off.
    /// </summary>
    public class FaultConsumer : IConsumer<Fault>
    {
        public Task Consume(ConsumeContext<Fault> context) => Task.CompletedTask;
    }
}
