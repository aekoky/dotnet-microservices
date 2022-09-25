using Formuler.Shared.DTO.UserService;
using System.Threading.Tasks;
using UserService.Data.Models;

namespace UserService.Business.Services
{
    public interface IUserService
    {
        Task<UserEntity> RegisterUser(RegisterUserRequestDTO registerUserRequest);
        Task<UserEntity> GetUser(string username);
    }
}
