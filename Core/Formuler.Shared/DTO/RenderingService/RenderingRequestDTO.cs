using System;
using System.ComponentModel.DataAnnotations;

namespace Formuler.Shared.DTO.RenderingService
{
    public class RenderingRequestDTO
    {
        [Required]
        public Guid TemplateId { get; set; }
        public string Data { get; set; }
    }
}
