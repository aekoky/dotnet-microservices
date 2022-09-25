using MongoDB.Bson;
using System;

namespace Formuler.Core.MessageBroker.Commands
{
    public class RenderingCommand
    {
        public Guid DocumentId { get; set; }
    }
}
