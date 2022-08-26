using UserService.Business.DTO.Requests;
using UserService.Data.Models;

namespace UserService.Business.Services
{
    public interface IUserService
    {
        UserEntity RegisterUser(RegisterUserRequest registerUserRequest);
        UserEntity GetUser(string username);
    }
}
