using Formuler.Shared.ApiFacade;
using Formuler.Shared.ApiFacade.ApiSettings;
using Formuler.Shared.DTO.FileService;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Formuler.Core.ApiFacade.FileService
{
    public class FileServiceApiFacade : IFileServiceApiFacade
    {
        private readonly FileServiceFacadeSettings _fileServiceFacadeSettings;
        private readonly RestClient _restClient;
        private readonly ILogger _logger;

        public FileServiceApiFacade(RestClientFactory restClientFactory, ILogger<FileServiceApiFacade> logger)
        {
            _fileServiceFacadeSettings = restClientFactory.FileServiceFacadeSettings;
            _restClient = restClientFactory.FileServiceRestClient;
            _logger = logger;
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

        public async Task<FileDto> DownloadFile(Guid fileId, bool retry = false)
        {
            var request = new RestRequest(_fileServiceFacadeSettings.DownloadFileUrl);
            request = request.AddUrlSegment("fileId", fileId);
            RestResponse<FileDto> response;
            if (retry)
                response = ExecuteRequest<FileDto>(request);
            else
                response = await _restClient.ExecuteAsync<FileDto>(request).ConfigureAwait(false);

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

        private RestResponse<T> ExecuteRequest<T>(RestRequest request)
        {
            var retryPolicy = Policy
                .HandleResult<RestResponse<T>>(x => (int)x.StatusCode >= 500)
                .WaitAndRetry(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(4, retryAttempt)),
                    (exception, timeSpan) =>
                    {
                        _logger.LogError(exception.Exception, $"Retrying the call {exception.Result}. Time: {timeSpan}");
                    });

            var circuitBreakerPolicy = Policy
                .HandleResult<RestResponse<T>>(x => x.StatusCode == HttpStatusCode.ServiceUnavailable)
                .CircuitBreaker(1, TimeSpan.FromSeconds(60), onBreak: (iRestResponse, timespan, context) =>
                {
                    _logger.LogWarning($"Circuit went into a fault state. Reason: {iRestResponse.Result.Content}");
                },
                onReset: (context) =>
                {
                    _logger.LogWarning($"Circuit left the fault state.");
                });

            return retryPolicy.Wrap(circuitBreakerPolicy).Execute(() => _restClient.Execute<T>(request));
        }
    }
}
