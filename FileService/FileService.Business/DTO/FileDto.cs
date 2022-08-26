using Microsoft.AspNetCore.Http;
using System;

namespace FileService.Business.DTO
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
