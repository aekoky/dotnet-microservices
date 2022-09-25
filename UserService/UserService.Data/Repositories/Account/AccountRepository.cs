using Formuler.Core.Repository;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System.Threading.Tasks;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public class AccountRepository : MongoRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(IMongoDbContext context) : base(context)
        {
        }

        public async Task<AccountEntity> GetAccountByUsername(string username)
        {
            var findOptions = new FindOptions<AccountEntity>() { Collation = new Collation("en", strength: CollationStrength.Secondary) };
            return await FindAsync(account => username.Equals(account.Username), findOptions).ConfigureAwait(false);
        }

        public async Task<AccountEntity> GetAccountByUsernameAndPassword(string username, string password)
        {
            var filter = Builders<AccountEntity>.Filter.Eq(account => account.Username, username) &
            Builders<AccountEntity>.Filter.Eq(account => account.Password, password);
            return await FindAsync(filter).ConfigureAwait(false);
        }
    }
}
