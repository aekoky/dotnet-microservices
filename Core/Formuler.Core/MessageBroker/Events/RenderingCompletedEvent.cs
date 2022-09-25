using Formuler.Core.Enums;
using System;

namespace Formuler.Core.MessageBroker.Events
{
    public class RenderingCompletedEvent
    {
        public Guid FileId { get; set; }
        public Guid DocumentId { get; set; }
        public RenderingStatus RenderingStatus { get; set; }
    }
}

