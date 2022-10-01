using Microsoft.AspNetCore.Http;

namespace TemplateService.Business.DTO.TemplateService
{
    public class CreateTemplateDTO
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public IFormFile File { get; set; }
        public string OwnerId { get; set; }
    }
}
