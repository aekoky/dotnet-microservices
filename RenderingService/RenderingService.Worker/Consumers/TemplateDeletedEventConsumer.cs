using Formuler.Core.MessageBroker.Events;
using MassTransit;
using RenderingService.Data.Repositories;
using System.Threading.Tasks;

namespace RenderingService.Worker.Consumers
{
    public class TemplateDeletedEventConsumer : IConsumer<TemplateDeletedEvent>
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateDeletedEventConsumer(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task Consume(ConsumeContext<TemplateDeletedEvent> context)
        {
            var templateCreatedEvent = context.Message;

            var template = await _templateRepository.FindAsync(templateCreatedEvent.TemplateId);
            if (template != null)
                await _templateRepository.DeleteOneAsync(template);
        }
    }
}
