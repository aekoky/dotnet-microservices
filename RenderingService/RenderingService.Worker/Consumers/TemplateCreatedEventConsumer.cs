using Formuler.Core.MessageBroker.Events;
using MassTransit;
using RenderingService.Data.Models;
using RenderingService.Data.Repositories;
using System.Threading.Tasks;

namespace RenderingService.Worker.Consumers
{
    public class TemplateCreatedEventConsumer : IConsumer<TemplateCreatedEvent>
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateCreatedEventConsumer(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task Consume(ConsumeContext<TemplateCreatedEvent> context)
        {
            var templateCreatedEvent = context.Message;

            var template = await _templateRepository.FindAsync(templateCreatedEvent.TemplateId);
            if (template != null)
                await _templateRepository.DeleteOneAsync(template);

            await _templateRepository.AddOneAsync(new TemplateEntity
            {
                Id = templateCreatedEvent.TemplateId,
                FileId = templateCreatedEvent.FileId
            });
        }
    }
}
