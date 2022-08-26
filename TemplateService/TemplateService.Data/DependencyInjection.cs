using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;
using TemplateService.Data.Repositories;

namespace TemplateService.Data
{
    public static class DependencyInjection
    {
        public static void AddTemplateServiceData(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMongoDbContext, TemplateServiceDbContext>();
            serviceCollection.AddTransient<ITemplateRepository, TemplateRepository>();
        }
    }
}
