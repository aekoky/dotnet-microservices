using FileService.Business.DTO;
using FileService.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IStorageService _storageService;

        public FileController(ILogger<FileController> logger, IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<FileDto> GetFile(Guid fileId)
        {
            return await _storageService.DownloadFile(fileId);
        }

        [HttpDelete]
        public async Task DeleteFile(Guid fileId)
        {
            await _storageService.DeleteFile(fileId);
        }

        [HttpPost]
        public async Task SaveFile([FromForm]SaveFileDto file)
        {
            await _storageService.SaveFile(file);
        }
    }
}
