using System;

namespace Formuler.Core.MessageBroker.Events
{
    public class TemplateCreatedEvent
    {
        public Guid TemplateId { get; set; }
        public Guid FileId { get; set; }
    }
}
