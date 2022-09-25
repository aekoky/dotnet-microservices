using Formuler.Shared.DTO.FileService;
using System;
using System.Threading.Tasks;

namespace FileService.Business.Services
{
    public interface IStorageService
    {
        Task DeleteFile(Guid fileId);
        Task SaveFile(SaveFileDto file);
        Task<FileDto> DownloadFile(Guid guid);
    }
}
