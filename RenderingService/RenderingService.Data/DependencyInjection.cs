using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;
using RenderingService.Data.Repositories;

namespace RenderingService.Data
{
    public static class DependencyInjection
    {
        public static void AddRenderingServiceeData(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDocumentDataRepository, DocumentDataRepository>();
            serviceCollection.AddTransient<IDocumentHistoryRepository, DocumentHistoryRepository>();
            serviceCollection.AddTransient<IDocumentRepository, DocumentRepository>();
            serviceCollection.AddTransient<ITemplateRepository, TemplateRepository>();
            serviceCollection.AddTransient<IMongoDbContext, RenderingServiceDbContext>();
        }
    }
}
