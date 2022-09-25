using Formuler.Shared.DTO.TemplateService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateService.Business.Services;

namespace TemplateService.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet("{templateId:guid}")]
        public async Task<TemplateDTO> GetTemplate([FromRoute] Guid templateId)
        {
            return await _templateService.GetTemplate(templateId);
        }

        [HttpGet]
        public async Task<IList<TemplateDTO>> GetTemplates([FromQuery] string keyword)
        {
            return await _templateService.GetTemplates(new TemplateFilterDTO { Keyword = keyword });
        }

        [HttpPost]
        public async Task<TemplateDTO> CreateTemplate([FromForm] CreateTemplateDTO template)
        {
            return await _templateService.CreateTemplate(template);
        }

        [HttpPut]
        public async Task<TemplateDTO> UpdateTemplate([FromBody] UpdateTemplateDTO template)
        {
            return await _templateService.UpdateTemplate(template);
        }

        [HttpDelete("{templateId:guid}")]
        public async Task DropTemplate([FromRoute] Guid templateId)
        {
            await _templateService.DropTemplate(templateId);
        }
    }
}
