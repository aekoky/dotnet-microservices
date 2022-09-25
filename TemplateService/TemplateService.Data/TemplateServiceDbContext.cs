using Formuler.Core.Repository;
using Microsoft.Extensions.Options;
using MongoDbGenericRepository;

namespace TemplateService.Data
{
    public class TemplateServiceDbContext : MongoDbContext
    {
        public TemplateServiceDbContext(IOptions<MongoDbSettings> mongoDbSettingsOption) : base(mongoDbSettingsOption.Value.ConnectionString, mongoDbSettingsOption.Value.DatabaseName)
        {
        }
    }
}
