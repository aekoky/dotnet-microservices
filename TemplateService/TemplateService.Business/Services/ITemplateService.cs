using System;
using System.Collections.Generic;
using TemplateService.Business.DTO;

namespace TemplateService.Business.Services
{
    public interface ITemplateService
    {
        TemplateDTO GetTemplate(Guid templateId);
        IList<TemplateDTO> GetTemplates();
        TemplateDTO CreateTemplate(CreateTemplateDTO template);
        TemplateDTO UpdateTemplate(UpdateTemplateDTO template);
        void DropTemplate(Guid templateId);
    }
}
