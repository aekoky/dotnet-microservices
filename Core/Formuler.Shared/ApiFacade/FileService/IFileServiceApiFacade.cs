using Formuler.Shared.DTO.FileService;
using System;
using System.Threading.Tasks;

namespace Formuler.Core.ApiFacade.FileService
{
    public interface IFileServiceApiFacade
    {
        Task DeleteFile(Guid fileId);
        Task SaveFile(SaveFileDto file);
        Task<FileDto> DownloadFile(Guid guid, bool retry = false);
    }
}
