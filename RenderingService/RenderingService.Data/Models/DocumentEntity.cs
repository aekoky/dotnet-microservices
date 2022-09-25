using Formuler.Core.Repository;
using System;

namespace RenderingService.Data.Models
{
    public class DocumentEntity : MongoEntity
    {
        public Guid DocumentDataId { get; set; }
        public Guid FileId { get; set; }
    }
}
