using Microsoft.AspNetCore.Http;
using System;

namespace FileService.Business.DTO
{
    public class SaveFileDto
    {
        public Guid Id { get; set; }
        public IFormFile Data { get; set; }
    }
}
