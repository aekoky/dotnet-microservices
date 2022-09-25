using Formuler.Core.Enums;
using Formuler.Core.Repository;
using System;

namespace RenderingService.Data.Models
{
    public class DocumentHistoryEntity : MongoEntity
    {
        public Guid DocumentId { get; set; }
        public RenderingStatus RenderingStatus { get; set; }
    }
}
