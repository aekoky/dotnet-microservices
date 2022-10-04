using DinkToPdf;
using DinkToPdf.Contracts;
using Formuler.Core.ApiFacade.FileService;
using Formuler.Shared.Enums;
using Formuler.Core.MessageBroker.Commands;
using Formuler.Core.MessageBroker.Events;
using Formuler.Shared.DTO.FileService;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using RenderingService.Data.Models;
using RenderingService.Data.Repositories;
using Scriban;
using System;
using System.Text;
using System.Threading.Tasks;
using Scriban.Runtime;
using System.Dynamic;
using System.Collections.Generic;
using Formuler.Core.Helpers;

namespace RenderingService.Worker.Consumers
{
    public class RenderingCommandConsumer : IConsumer<RenderingCommand>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentDataRepository _documentDataRepository;
        private readonly IDocumentHistoryRepository _documentHistoryRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IFileServiceApiFacade _fileServiceApiFacade;
        private readonly ILogger _logger;
        private readonly IConverter _converter;

        public RenderingCommandConsumer(IDocumentRepository documentRepository, IDocumentDataRepository documentDataRepository, IDocumentHistoryRepository documentHistoryRepository, ITemplateRepository templateRepository, IFileServiceApiFacade fileServiceApiFacade, ILogger<RenderingCommandConsumer> logger, IConverter converter)
        {
            _documentRepository = documentRepository;
            _documentDataRepository = documentDataRepository;
            _documentHistoryRepository = documentHistoryRepository;
            _templateRepository = templateRepository;
            _fileServiceApiFacade = fileServiceApiFacade;
            _logger = logger;
            _converter = converter;
        }

        public async Task Consume(ConsumeContext<RenderingCommand> context)
        {
            var renderingRequest = context.Message;
            try
            {
                if (renderingRequest is null || renderingRequest.DocumentId == Guid.Empty)
                    throw new Exception("Rendering Request not valide");

                var document = await _documentRepository.FindAsync(renderingRequest.DocumentId);
                if (document is null)
                    throw new Exception("Document not found");

                var documentData = await _documentDataRepository.FindAsync(document.DocumentDataId);
                if (documentData is null)
                    throw new Exception("Document data not found");

                await _documentHistoryRepository.AddOneAsync(new DocumentHistoryEntity
                {
                    DocumentId = document.Id,
                    RenderingStatus = RenderingStatus.Progressing
                });
                var template = await _templateRepository.FindAsync(documentData.TemplateId);
                if (template is null)
                    throw new Exception("Template not found");
                var templateFile = await _fileServiceApiFacade.DownloadFile(template.FileId, true);
                var htmlString = await ParseHtmlAsync(templateFile.Data, documentData.Data);
                var pdfDocument = ParsePdfAsync(htmlString);
                var savefiledto = new SaveFileDto { Id = document.Id, Data = pdfDocument };
                await _fileServiceApiFacade.SaveFile(savefiledto);
                document.FileId = savefiledto.Id;
                await _documentRepository.UpdateOneAsync(document);
                await _documentHistoryRepository.AddOneAsync(
                    new DocumentHistoryEntity
                    {
                        DocumentId = document.Id,
                        RenderingStatus = RenderingStatus.Successed
                    });

                await context.Publish(new RenderingCompletedEvent { RenderingStatus = RenderingStatus.Successed, DocumentId = document.Id, FileId = document.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while rendering a document", context);
                await _documentHistoryRepository.AddOneAsync(
                    new DocumentHistoryEntity
                    {
                        DocumentId = renderingRequest.DocumentId,
                        RenderingStatus = RenderingStatus.Failed
                    });
                await context.Publish(new RenderingCompletedEvent { RenderingStatus = RenderingStatus.Failed });
                throw;
            }
        }

        private async Task<string> ParseHtmlAsync(byte[] templateData, BsonDocument documentData)
        {
            var htmlTemplateString = Encoding.UTF8.GetString(templateData);
            var htmlTemplate = Template.Parse(htmlTemplateString);
            var expando = JsonConvert.DeserializeObject<ExpandoObject>(documentData.ToJson());
            var sObject = expando.BuildScriptObject();
            return await htmlTemplate.RenderAsync(new TemplateContext(sObject));
        }

        private byte[] ParsePdfAsync(string htmlString)
        {
            var documentDefinition = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Margins = new MarginSettings() { Top = 10 }
                    },
                Objects = {
                        new ObjectSettings() {
                            PagesCount = true,
                            HtmlContent = htmlString,
                            WebSettings = { DefaultEncoding = "utf-8" },
                            HeaderSettings = { FontSize = 9, Right = "Page [page] de [toPage]", Line = true, Spacing = 2.812 }
                        }
                    }
            };
            return _converter.Convert(documentDefinition);
        }
    }
}
