using System;

namespace Formuler.Core.MessageBroker.Events
{
    public class TemplateDeletedEvent
    {
        public Guid TemplateId { get; set; }
    }
}
