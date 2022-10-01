using Formuler.Core.ApiFacade.FileService;
using Formuler.Core.MessageBroker.Events;
using Formuler.Shared.DTO.FileService;
using Formuler.WebCore.JWT;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TemplateService.Business.DTO.TemplateService;
using TemplateService.Data.Models;
using TemplateService.Data.Repositories;

namespace TemplateService.Business.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IFileServiceApiFacade _fileServiceApiFacade;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPublishEndpoint _publishEndpoint;

        public TemplateService(ITemplateRepository templateRepository, IFileServiceApiFacade fileServiceApiFacade, IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint)
        {
            _templateRepository = templateRepository;
            _fileServiceApiFacade = fileServiceApiFacade;
            _httpContextAccessor = httpContextAccessor;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<TemplateDTO> CreateTemplate(CreateTemplateDTO template)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid fileId = Guid.Empty;
            if (template.File != null || template.File.Length > 0)
            {
                fileId = Guid.NewGuid();
                var memoryStream = new MemoryStream();
                await template.File.OpenReadStream().CopyToAsync(memoryStream);
                //byte[] fileData=template.File
                var file = new SaveFileDto
                {
                    Id = fileId,
                    Data = memoryStream.ToArray()
                };
                await _fileServiceApiFacade.SaveFile(file).ConfigureAwait(false);
            }
            var templateEntity = new TemplateEntity
            {
                Description = template.Description,
                Details = template.Details,
                Label = template.Label,
                OwnerId = userId,
                FileId = fileId

            };
            await _templateRepository.AddOneAsync(templateEntity).ConfigureAwait(false);

            await _publishEndpoint.Publish(new TemplateCreatedEvent
            {
                TemplateId = templateEntity.Id,
                FileId = templateEntity.FileId
            });

            return new TemplateDTO
            {
                Id = templateEntity.Id,
                Description = templateEntity.Description,
                OwnerId = templateEntity.OwnerId,
                Details = templateEntity.Details,
                FileId = templateEntity.FileId,
                Label = templateEntity.Label
            };
        }

        public async Task DropTemplate(Guid templateId)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var templateEntity = await _templateRepository.FindAsync(templateId).ConfigureAwait(false);

            if (templateEntity is null)
            {
                throw new InvalidOperationException($"The template with the id {templateEntity.Id} does not exist");
            }

            if (templateEntity.OwnerId != userId && userRole != UserRoles.Admin)
            {
                throw new InvalidOperationException("The current user don't own the template");
            }

            if (templateEntity.FileId != Guid.Empty)
            {
                await _fileServiceApiFacade.DeleteFile(templateEntity.FileId).ConfigureAwait(false);
            }

            await _templateRepository.DeleteOneAsync(templateEntity).ConfigureAwait(false);

            await _publishEndpoint.Publish(new TemplateDeletedEvent
            {
                TemplateId = templateId
            });
        }

        public async Task<TemplateDTO> GetTemplate(Guid templateId)
        {
            var templateEntity = await _templateRepository.FindAsync(templateId).ConfigureAwait(false);

            if (templateEntity is null)
            {
                throw new InvalidOperationException($"The template with the id {templateId} does not exist");
            }

            return new TemplateDTO
            {
                Id = templateEntity.Id,
                Description = templateEntity.Description,
                OwnerId = templateEntity.OwnerId,
                Details = templateEntity.Details,
                FileId = templateEntity.FileId,
                Label = templateEntity.Label,
                ThumbnailLocation = templateEntity.ThumbnailLocation
            };
        }

        public async Task<IList<TemplateDTO>> GetTemplates(TemplateFilterDTO templateFilterDTO)
        {
            var templates = await _templateRepository.FilterTemplates(templateFilterDTO.Keyword);

            return templates.Select(template => new TemplateDTO
            {
                Id = template.Id,
                Description = template.Description,
                OwnerId = template.OwnerId,
                Details = template.Details,
                FileId = template.FileId,
                Label = template.Label,
                ThumbnailLocation = template.ThumbnailLocation
            }).ToList();
        }

        public async Task<TemplateDTO> UpdateTemplate(UpdateTemplateDTO template)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var templateEntity = await _templateRepository.FindAsync(template.Id).ConfigureAwait(false);

            if (templateEntity is null)
            {
                throw new InvalidOperationException($"The template with the id {templateEntity.Id} does not exist");
            }

            if (templateEntity.OwnerId != userId && userRole != UserRoles.Admin)
            {
                throw new InvalidOperationException("The current user don't own the template");
            }

            templateEntity.Label = template.Label;
            templateEntity.Description = template.Description;
            templateEntity.Details = template.Details;

            await _templateRepository.UpdateOneAsync(templateEntity);

            return new TemplateDTO
            {
                Id = templateEntity.Id,
                Description = templateEntity.Description,
                OwnerId = templateEntity.OwnerId,
                Details = templateEntity.Details,
                FileId = templateEntity.FileId,
                Label = templateEntity.Label,
                ThumbnailLocation = templateEntity.ThumbnailLocation
            };
        }
    }
}
