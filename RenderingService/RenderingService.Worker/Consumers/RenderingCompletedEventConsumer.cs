using Formuler.Core.MessageBroker.Events;
using MassTransit;
using System.Threading.Tasks;

namespace RenderingService.Worker.Consumers
{
    public class RenderingCompletedEventConsumer : IConsumer<RenderingCompletedEvent>
    {
        public Task Consume(ConsumeContext<RenderingCompletedEvent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
