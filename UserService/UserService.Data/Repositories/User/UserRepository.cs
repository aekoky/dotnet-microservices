using Core.Repository;
using MongoDbGenericRepository;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public class UserRepository : MongoRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IMongoDbContext context) : base(context)
        {
        }

    }
}
