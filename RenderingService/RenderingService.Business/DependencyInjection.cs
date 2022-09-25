using Formuler.Core.ApiFacade.FileService;
using Microsoft.Extensions.DependencyInjection;
using RenderingService.Business.Services;

namespace RenderingService.Business
{
    public static class DependencyInjection
    {
        public static void AddRenderingServiceBusiness(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFileServiceApiFacade, FileServiceApiFacade>();
            serviceCollection.AddTransient<IDocumentService, DocumentService>();
        }
    }
}
