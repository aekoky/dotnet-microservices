using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserService.Business.DTO.Requests;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public UserEntity GetUser(string username)
        {
            var account = _accountRepository.GetAccountByUsername(username);
            if (account is null)
                throw new ProblemDetailsException(StatusCodes.Status404NotFound, "No user were found with the giben username");

            return _userRepository.Find(account.UserId);
        }

        public UserEntity RegisterUser(RegisterUserRequest registerUserRequest)
        {
            var user = new UserEntity
            {
                Email = registerUserRequest.Email,
                Firstname = registerUserRequest.Firstname,
                Lastname = registerUserRequest.Lastname,
                PhoneNumber = registerUserRequest.PhoneNumber
            };

            _userRepository.AddOne(user);

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
            _accountRepository.AddOne(account);

            return user;
        }
    }
}
