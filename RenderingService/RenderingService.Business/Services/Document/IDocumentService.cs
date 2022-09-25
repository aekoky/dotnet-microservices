using Formuler.Shared.DTO.RenderingService;
using System;
using System.Threading.Tasks;

namespace RenderingService.Business.Services
{
    public interface IDocumentService
    {
        Task<Guid> CreateRenderingRequest(RenderingRequestDTO renderingRequest);
        Task<RenderingStatusResult> GetRenderingStatus(Guid documentId);
    }
}