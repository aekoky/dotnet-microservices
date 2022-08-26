using Core.Repository;

namespace Core.Models
{
    public class RenderedEntity : MongoEntity
    {
        public string RenderedFileName { get; set; }
        public string TemplateId { get; set; }
        public string DataId { get; set; }
        public string OwnerId { get; set; }
        public string RenderingStatus { get; set; }
    }
}
