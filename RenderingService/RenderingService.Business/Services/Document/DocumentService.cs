using MassTransit;
using System.Threading.Tasks;
using Formuler.Shared.DTO.RenderingService;
using RenderingService.Data.Repositories;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using RenderingService.Data.Models;
using Formuler.Shared.Enums;
using Formuler.Core.MessageBroker.Commands;
using System;
using Formuler.Core.ApiFacade.FileService;

namespace RenderingService.Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentHistoryRepository _documentHistoryRepository;
        private readonly IDocumentDataRepository _documentDataRepository;
        private readonly IFileServiceApiFacade _fileServiceApiFacade;

        public DocumentService(ILogger<DocumentService> logger, IPublishEndpoint publishEndpoint, IDocumentRepository documentRepository, IDocumentHistoryRepository documentHistoryRepository, IDocumentDataRepository documentDataRepository, IFileServiceApiFacade fileServiceApiFacade)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _documentRepository = documentRepository;
            _documentHistoryRepository = documentHistoryRepository;
            _documentDataRepository = documentDataRepository;
            _fileServiceApiFacade = fileServiceApiFacade;
        }

        public async Task<Guid> CreateRenderingRequest(RenderingRequestDTO renderingRequest)
        {
            if (!BsonDocument.TryParse(renderingRequest.Data, out BsonDocument bsonData))
            {
                _logger.LogInformation("the json of document data couldn't parsed as bson");
                throw new ProblemDetailsException(StatusCodes.Status400BadRequest, $"Invalide JSON.");
            }
            var documentData = new DocumentDataEntity
            {
                Data = bsonData,
                TemplateId = renderingRequest.TemplateId
            };

            await _documentDataRepository.AddOneAsync(documentData);

            var document = new DocumentEntity
            {
                DocumentDataId = documentData.Id
            };

            await _documentRepository.AddOneAsync(document);

            var documentHistory = new DocumentHistoryEntity
            {
                DocumentId = document.Id,
                RenderingStatus = RenderingStatus.Pending
            };

            await _documentHistoryRepository.AddOneAsync(documentHistory);

            await _publishEndpoint.Publish(new RenderingCommand
            {
                DocumentId = document.Id
            });

            return document.Id;
        }

        public async Task<RenderingStatusResult> GetRenderingStatus(Guid documentId)
        {
            byte[] file = new byte[0];
            var documentHistory = await _documentHistoryRepository.GetByMaxAsync<DocumentHistoryEntity>(dh => dh.DocumentId == documentId, dh => dh.AddedAtUtc);

            if (documentHistory.RenderingStatus == RenderingStatus.Successed)
            {
                var documentEntity = await _documentRepository.FindAsync(documentId);
                if(documentEntity is null)
                {
                    _logger.LogError("Document not found");
                    throw new ProblemDetailsException(StatusCodes.Status404NotFound, $"Document not found.");
                }
                if (!documentEntity.FileId.Equals(Guid.Empty))
                {
                    var downloadedFile = await _fileServiceApiFacade.DownloadFile(documentEntity.FileId, true);
                    file = downloadedFile.Data;
                }
            }

            return new RenderingStatusResult
            {
                RenderingStatus = documentHistory.RenderingStatus,
                File = file
            };
        }
    }
}
