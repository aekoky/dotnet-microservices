using System.Threading.Tasks;
using UserService.Business.DTO.Requests;
using UserService.Business.DTO.Results;
using UserService.Data.Models;

namespace UserService.Business.Services
{
    public interface IAccountService
    {
        LoginResult Login(LoginRequest request);
        Task<LoginResult> RefreshToken(RefreshTokenRequest request);
        LoginResult Impersonate(ImpersonationRequest request);
        LoginResult StopImpersonation();
        bool IsValidUserCredentials(string userName, string password);
        AccountEntity GetAccount(string username);
    }
}
