using Microsoft.Extensions.DependencyInjection;
using TemplateService.Business.Services;

namespace TemplateService.Business
{
    public static class DependencyInjection
    {
        public static void AddTemplateServiceBusiness(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITemplateService, Services.TemplateService>();
        }
    }
}
