using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateService.Business.DTO.TemplateService;

namespace TemplateService.Business.Services
{
    public interface ITemplateService
    {
        Task<TemplateDTO> GetTemplate(Guid templateId);
        Task<IList<TemplateDTO>> GetTemplates(TemplateFilterDTO templateFilterDTO);
        Task<TemplateDTO> CreateTemplate(CreateTemplateDTO template);
        Task<TemplateDTO> UpdateTemplate(UpdateTemplateDTO template);
        Task DropTemplate(Guid templateId);
    }
}
