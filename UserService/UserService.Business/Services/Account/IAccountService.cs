using Formuler.Shared.DTO.UserService;
using System.Threading.Tasks;
using UserService.Data.Models;

namespace UserService.Business.Services
{
    public interface IAccountService
    {
        Task<LoginResultDTO> Login(LoginRequestDTO request);
        Task<LoginResultDTO> RefreshToken(RefreshTokenRequestDTO request);
        Task<LoginResultDTO> Impersonate(ImpersonationRequestDTO request);
        Task<LoginResultDTO> StopImpersonation();
        Task<bool> IsValidUserCredentials(string userName, string password);
        Task<AccountEntity> GetAccount(string username);
    }
}
