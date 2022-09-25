using Formuler.Core.Repository;
using System;

namespace TemplateService.Data.Models
{
    public class TemplateEntity : MongoEntity
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public Guid FileId { get; set; }
        public string ThumbnailLocation { get; set; }
        public string OwnerId { get; set; }
    }
}
