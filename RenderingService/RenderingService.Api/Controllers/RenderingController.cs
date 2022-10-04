using Formuler.Shared.DTO.RenderingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenderingService.Business.Services;
using System;
using System.Threading.Tasks;

namespace RenderingService.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class RenderingController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public RenderingController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<Guid> CreateRenderingRequest([FromBody] RenderingRequestDTO renderingRequestDTO)
        {
            return await _documentService.CreateRenderingRequest(renderingRequestDTO);
        }

        [HttpGet("{documentId:guid}")]
        public async Task<RenderingStatusResult> GetFile([FromRoute] Guid documentId)
        {
            return await _documentService.GetRenderingStatus(documentId);
        }
    }
}
