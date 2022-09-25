using Formuler.Shared.DTO.FileService;
using Microsoft.Extensions.Logging;
using Storage.Net.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileService.Business.Services
{
    public class StorageService : IStorageService
    {
        private readonly IBlobStorage _blobStorage;
        private readonly ILogger<StorageService> _logger;

        public StorageService(IBlobStorage blobStorage, ILogger<StorageService> logger)
        {
            _blobStorage = blobStorage;
            _logger = logger;
        }

        public async Task DeleteFile(Guid fileId)
        {
            string path = fileId.ToString();
            if (await _blobStorage.ExistsAsync(path).ConfigureAwait(false))
            {
                await _blobStorage.DeleteAsync(path).ConfigureAwait(false);
                _logger.LogInformation($"File {path} have been deleted");

                return;
            }
            _logger.LogWarning($"File {path} does not exist");
            throw new InvalidOperationException($"File {path} does not exist");
        }

        public async Task<FileDto> DownloadFile(Guid fileId)
        {
            string path = fileId.ToString();
            if (await _blobStorage.ExistsAsync(path).ConfigureAwait(false))
            {
                _logger.LogInformation($"File {path} downloaded");
                var data = await _blobStorage.ReadBytesAsync(path).ConfigureAwait(false);

                return new FileDto
                {
                    Id = fileId,
                    Data = data,
                    Size = data.Length
                };
            }

            _logger.LogWarning($"File {path} does not exist");
            throw new InvalidOperationException($"File {path} does not exist");
        }

        public async Task SaveFile(SaveFileDto file)
        {
            string path = file.Id.ToString();
            if (await _blobStorage.ExistsAsync(path).ConfigureAwait(false))
            {
                _logger.LogWarning($"File {path} does not exist");
                throw new InvalidOperationException($"File {path} does not exist");
            }

            await _blobStorage.SetBlobAsync(new Blob(path)).ConfigureAwait(false);
            await _blobStorage.WriteAsync(path, new MemoryStream(file.Data)).ConfigureAwait(false);
            _logger.LogInformation($"File {path} have benn saved");
        }
    }
}
