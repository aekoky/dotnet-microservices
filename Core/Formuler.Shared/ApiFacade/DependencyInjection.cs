using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Formuler.Shared.ApiFacade
{
    public static class DependencyInjection
    {
        public static void AddRestClient(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            if (isDevelopment)
                Environment.SetEnvironmentVariable("Certificates_Default_Password", configuration.GetSection("Kestrel:Certificates:Development:Password").Value);
            serviceCollection.AddSingleton<RestClientFactory>();
        }
    }
}
