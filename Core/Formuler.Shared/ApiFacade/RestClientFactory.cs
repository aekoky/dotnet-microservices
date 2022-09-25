using Formuler.Shared.ApiFacade.ApiSettings;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Formuler.Shared.ApiFacade
{
    public class RestClientFactory
    {
        private static RestClient _fileServiceRestClient;
        private static FileServiceFacadeSettings _fileServiceFacadeSettings;

        public RestClientFactory(IOptions<FileServiceFacadeSettings> fileServiceFacadeSettings)
        {
            _fileServiceFacadeSettings = fileServiceFacadeSettings.Value;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public FileServiceFacadeSettings FileServiceFacadeSettings => _fileServiceFacadeSettings;

        public RestClient FileServiceRestClient
        {
            get
            {
                if (_fileServiceRestClient is null)
                    _fileServiceRestClient = InitializeRestClient(_fileServiceFacadeSettings.BaseUrl);

                return _fileServiceRestClient;
            }
        }

        private RestClient InitializeRestClient(string baseUrl)
        {
            var baseUri = new Uri(baseUrl);

            var certificatePath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
            if (certificatePath is null)
                certificatePath = Environment.GetEnvironmentVariable("Certificates_Default_Path");

            var certificatePassword = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
            if (certificatePassword is null)
                certificatePassword = Environment.GetEnvironmentVariable("Certificates_Default_Password");

            var certFile = Path.Combine(Path.GetDirectoryName(certificatePath), Path.GetFileName(certificatePath));
            X509Certificate2 certificate = new X509Certificate2(certFile, certificatePassword);
            var clientOptions = new RestClientOptions
            {
                BaseHost = baseUri.Host,
                BaseUrl = baseUri,
                ClientCertificates = new X509CertificateCollection() { certificate }
            };


            clientOptions.RemoteCertificateValidationCallback +=
                new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) =>
                {
                    const SslPolicyErrors ignoredErrors =
                               SslPolicyErrors.RemoteCertificateChainErrors |  // self-signed
                               SslPolicyErrors.RemoteCertificateNameMismatch;  // name mismatch
                    return (policyErrors & ~ignoredErrors) == SslPolicyErrors.None;
                });
            return new RestClient(clientOptions);
        }
    }
}
