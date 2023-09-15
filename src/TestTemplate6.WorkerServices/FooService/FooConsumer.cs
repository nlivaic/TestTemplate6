using System.Threading.Tasks;
using MassTransit;
using TestTemplate6.Core.Events;

namespace TestTemplate6.WorkerServices.FooService
{
    public class FooConsumer : IConsumer<IFooEvent>
    {
        public Task Consume(ConsumeContext<IFooEvent> context) =>
            Task.CompletedTask;
    }
}
