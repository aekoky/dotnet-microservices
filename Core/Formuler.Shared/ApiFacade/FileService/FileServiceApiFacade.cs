using Formuler.Shared.ApiFacade;
using Formuler.Shared.ApiFacade.ApiSettings;
using Formuler.Shared.DTO.FileService;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Formuler.Core.ApiFacade.FileService
{
    public class FileServiceApiFacade : IFileServiceApiFacade
    {
        private readonly FileServiceFacadeSettings _fileServiceFacadeSettings;
        private readonly RestClient _restClient;

        public FileServiceApiFacade(RestClientFactory restClientFactory)
        {
            _fileServiceFacadeSettings = restClientFactory.FileServiceFacadeSettings;
            _restClient = restClientFactory.FileServiceRestClient;
        }

        public async Task DeleteFile(Guid fileId)
        {
            var request = new RestRequest(_fileServiceFacadeSettings.DeleteFileUrl, Method.Delete);
            request = request.AddUrlSegment("fileId", fileId);
            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            if (response.IsSuccessful)
                return;
            throw response.ErrorException;
        }

        public async Task<FileDto> DownloadFile(Guid fileId)
        {
            var request = new RestRequest(_fileServiceFacadeSettings.DownloadFileUrl);
            request = request.AddUrlSegment("fileId", fileId);
            var response = await _restClient.ExecuteAsync<FileDto>(request).ConfigureAwait(false);
            if (response.IsSuccessful)
                return response.Data;
            throw response.ErrorException;
        }

        public async Task SaveFile(SaveFileDto file)
        {
            var request = new RestRequest(_fileServiceFacadeSettings.SaveFileUrl, Method.Post)
                .AddJsonBody(file);
            var response = await _restClient.ExecuteAsync(request).ConfigureAwait(false);
            if (response.IsSuccessful)
                return;
            throw response.ErrorException;
        }
    }
}
