using System;

namespace Formuler.Shared.DTO.FileService
{
    public class SaveFileDto
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
    }
}
