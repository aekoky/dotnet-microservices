using Formuler.Core.Repository;
using MongoDB.Bson;
using System;

namespace RenderingService.Data.Models
{
    public class DocumentDataEntity : MongoEntity
    {
        public Guid TemplateId { get; set; }
        public Guid OwnerId { get; set; }
        public BsonDocument Data { get; set; }
    }
}
