using Formuler.Shared.DTO.UserService;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public UserService(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public async Task<UserEntity> GetUser(string username)
        {
            var account = await _accountRepository.GetAccountByUsername(username).ConfigureAwait(false);
            if (account is null)
                throw new ProblemDetailsException(StatusCodes.Status404NotFound, "No user were found with the giben username");

            return await _userRepository.FindAsync(account.UserId).ConfigureAwait(false);
        }

        public async Task<UserEntity> RegisterUser(RegisterUserRequestDTO registerUserRequest)
        {
            var user = new UserEntity
            {
                Email = registerUserRequest.Email,
                Firstname = registerUserRequest.Firstname,
                Lastname = registerUserRequest.Lastname,
                PhoneNumber = registerUserRequest.PhoneNumber
            };

            await _userRepository.AddOneAsync(user).ConfigureAwait(false);

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUserRequest.Password, salt);

            var account = new AccountEntity
            {
                Password = hashedPassword,
                Username = registerUserRequest.Username,
                Salt = salt,
                UserId = user.Id,
                Role = registerUserRequest.Role
            };
            await _accountRepository.AddOneAsync(account).ConfigureAwait(false);

            return user;
        }
    }
}
