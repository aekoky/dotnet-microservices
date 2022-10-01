using System;

namespace TemplateService.Business.DTO.TemplateService
{
    public class TemplateDTO
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public Guid FileId { get; set; }
        public string ThumbnailLocation { get; set; }
        public string OwnerId { get; set; }
    }
}
