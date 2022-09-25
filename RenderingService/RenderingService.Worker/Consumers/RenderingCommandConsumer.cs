using DinkToPdf;
using DinkToPdf.Contracts;
using Formuler.Core.ApiFacade.FileService;
using Formuler.Core.Enums;
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

namespace RenderingService.Worker.Consumers
{
    public class RenderingCommandConsumer : IConsumer<RenderingCommand>
    {
        private readonly IDocumentDataRepository _documentDataRepository;
        private readonly IDocumentHistoryRepository _documentHistoryRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IFileServiceApiFacade _fileServiceApiFacade;
        private readonly ILogger _logger;
        private readonly IConverter _converter;

        public RenderingCommandConsumer(IDocumentDataRepository documentDataRepository, IDocumentHistoryRepository documentHistoryRepository, ITemplateRepository templateRepository, IFileServiceApiFacade fileServiceApiFacade, ILogger<RenderingCommandConsumer> logger, IConverter converter)
        {
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
                if(renderingRequest is null || renderingRequest.DocumentId == Guid.Empty)
                    throw new Exception("Rendering Request not valide");

                var document = await _documentDataRepository.FindAsync(renderingRequest.DocumentId);
                if (document is null)
                    throw new Exception("Document not found");

                await _documentHistoryRepository.AddOneAsync(
                    new DocumentHistoryEntity
                    {
                        DocumentId = document.Id,
                        RenderingStatus = RenderingStatus.Progressing
                    });
                var template = await _templateRepository.FindAsync(document.TemplateId);
                if (template is null)
                    throw new Exception("Template not found");
                var templateFile = await _fileServiceApiFacade.DownloadFile(template.FileId);
                var htmlTemplateString = Encoding.UTF8.GetString(templateFile.Data);
                var htmlTemplate = Template.Parse(htmlTemplateString);
                var templateData = JsonConvert.DeserializeObject(document.Data.ToJson());
                var htmlString = await htmlTemplate.RenderAsync(templateData);
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
                            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                        }
                    }
                };
                var pdfDocument = _converter.Convert(documentDefinition);
                await _fileServiceApiFacade.SaveFile(new SaveFileDto { Id = document.Id, Data = pdfDocument });

                await _documentHistoryRepository.AddOneAsync(
                    new DocumentHistoryEntity
                    {
                        DocumentId = renderingRequest.DocumentId,
                        RenderingStatus = RenderingStatus.Successed
                    });
                await context.Publish(new RenderingCompletedEvent { RenderingStatus = RenderingStatus.Successed, DocumentId = document.Id, FileId = document.Id });

                return;
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
    }
}
