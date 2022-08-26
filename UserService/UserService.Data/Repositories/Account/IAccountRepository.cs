using Core.Repository;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public interface IAccountRepository : IMongoRepository<AccountEntity>
    {
        AccountEntity GetAccountByUsername(string username);
        AccountEntity GetAccountByUsernameAndPassword(string username, string password);

    }
}
