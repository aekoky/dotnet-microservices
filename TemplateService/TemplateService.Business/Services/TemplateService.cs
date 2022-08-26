using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TemplateService.Business.DTO;
using TemplateService.Data.Models;
using TemplateService.Data.Repositories;

namespace TemplateService.Business.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ILogger<TemplateService> _logger;

        public TemplateService(ITemplateRepository templateRepository, ILogger<TemplateService> logger)
        {
            _templateRepository = templateRepository;
            _logger = logger;
        }

        public TemplateDTO CreateTemplate(CreateTemplateDTO template)
        {
            throw new NotImplementedException();
        }

        public void DropTemplate(Guid templateId)
        {
            throw new NotImplementedException();
        }

        public TemplateDTO GetTemplate(Guid templateId)
        {
            throw new NotImplementedException();
        }

        public IList<TemplateDTO> GetTemplates()
        {
            throw new NotImplementedException();
        }

        public TemplateDTO UpdateTemplate(UpdateTemplateDTO template)
        {
            throw new NotImplementedException();
        }
    }
}
