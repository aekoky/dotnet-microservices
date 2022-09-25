using System;

namespace Formuler.Shared.DTO.TemplateService
{
    public class UpdateTemplateDTO
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
    }
}
