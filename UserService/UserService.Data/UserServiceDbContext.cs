using Formuler.Core.Repository;
using Microsoft.Extensions.Options;
using MongoDbGenericRepository;

namespace UserService.Data
{
    public class UserServiceDbContext : MongoDbContext
    {
        public UserServiceDbContext(IOptions<MongoDbSettings> mongoDbSettingsOption) : base(mongoDbSettingsOption.Value.ConnectionString, mongoDbSettingsOption.Value.DatabaseName)
        {
        }
    }
}
