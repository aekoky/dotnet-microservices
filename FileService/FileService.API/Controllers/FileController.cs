using FileService.Business.Services;
using Formuler.Shared.DTO.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public FileController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("{fileId:guid}")]
        public async Task<FileDto> GetFile([FromRoute] Guid fileId)
        {
            return await _storageService.DownloadFile(fileId);
        }

        [HttpDelete("{fileId:guid}")]
        public async Task DeleteFile([FromRoute] Guid fileId)
        {
            await _storageService.DeleteFile(fileId);
        }

        [HttpPost]
        public async Task SaveFile([FromBody] SaveFileDto file)
        {
            await _storageService.SaveFile(file);
        }
    }
}
