using System.Threading.Tasks;
using MassTransit;
using TestTemplate6.Core.Events;

namespace TestTemplate6.WorkerServices.FooService
{
    /// <summary>
    /// This is here only for show.
    /// I have not thought through a proper error handling strategy.
    /// Make FooConsumer throw in order to kick error handling off.
    /// </summary>
    public class FooFaultConsumer : IConsumer<Fault<IFooEvent>>
    {
        public Task Consume(ConsumeContext<Fault<IFooEvent>> context) =>
            Task.CompletedTask;
    }
}
