using Formuler.Core.Repository;
using System;

namespace RenderingService.Data.Models
{
    public class TemplateEntity : MongoEntity
    {
        public Guid FileId { get; set; }
    }
}
