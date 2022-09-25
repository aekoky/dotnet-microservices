using Formuler.Core.Repository;
using Microsoft.Extensions.Options;
using MongoDbGenericRepository;

namespace RenderingService.Data
{
    public class RenderingServiceDbContext : MongoDbContext
    {
        public RenderingServiceDbContext(IOptions<MongoDbSettings> mongoDbSettingsOption) : base(mongoDbSettingsOption.Value.ConnectionString, mongoDbSettingsOption.Value.DatabaseName)
        {
        }
    }
}
