using Formuler.Core.Repository;
using System.Threading.Tasks;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public interface IAccountRepository : IMongoRepository<AccountEntity>
    {
        Task<AccountEntity> GetAccountByUsername(string username);
        Task<AccountEntity> GetAccountByUsernameAndPassword(string username, string password);

    }
}
