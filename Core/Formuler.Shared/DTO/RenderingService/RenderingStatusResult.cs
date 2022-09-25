using Formuler.Core.Enums;

namespace Formuler.Shared.DTO.RenderingService
{
    public class RenderingStatusResult
    {
        public RenderingStatus RenderingStatus { get; set; }
        public byte[] File { get; set; }
    }
}
