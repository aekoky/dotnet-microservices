using Formuler.Core.Repository;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public interface IUserRepository : IMongoRepository<UserEntity>
    {
    }
}
