using Core.Repository;
using MongoDB.Driver;
using MongoDbGenericRepository;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public class AccountRepository : MongoRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(IMongoDbContext context) : base(context)
        {
        }

        public AccountEntity GetAccountByUsername(string username)
        {
            return Find(account => username.Equals(account.Username), new FindOptions() { Collation = new Collation("en", strength: CollationStrength.Secondary) });
        }

        public AccountEntity GetAccountByUsernameAndPassword(string username, string password)
        {
            var filter = Builders<AccountEntity>.Filter.Eq(account => account.Username, username) &
            Builders<AccountEntity>.Filter.Eq(account => account.Password, password);
            return Find(filter);
        }
    }
}
