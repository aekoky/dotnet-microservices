using Microsoft.Extensions.DependencyInjection;
using FileService.Business.Services;
using Storage.Net;
using Microsoft.Extensions.Options;

namespace FileService.Business
{
    public static class DependencyInjection
    {
        public static void AddFileServiceBusiness(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IStorageService, StorageService>();
            serviceCollection.AddSingleton((services) =>
            {
                var storageSettings = services.GetService<IOptions<StorageSettings>>().Value;
                return StorageFactory.Blobs.FromConnectionString(storageSettings.ConnectionString);
            });
        }
    }
}
