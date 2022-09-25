using System;

namespace Formuler.Shared.DTO.FileService
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
