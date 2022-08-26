using Core.JWT;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Business.DTO.Requests;
using UserService.Business.DTO.Results;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(ILogger<AccountService> logger, IAccountRepository accountRepository, IJwtAuthManager jwtAuthManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _jwtAuthManager = jwtAuthManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public AccountEntity GetAccount(string username)
        {
            return _accountRepository.GetAccountByUsername(username);
        }

        public LoginResult Impersonate(ImpersonationRequest request)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

            var impersonatedAccount = GetAccount(request.UserName);
            if (impersonatedAccount is null)
            {
                _logger.LogWarning($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
                throw new ProblemDetailsException(StatusCodes.Status400BadRequest, $"The target user [{request.UserName}] is not found.");
            }
            if (impersonatedAccount.Role == UserRoles.Admin)
            {
                _logger.LogWarning($"User [{userName}] is not allowed to impersonate another Admin.");
                throw new ProblemDetailsException(StatusCodes.Status400BadRequest, $"This action is not supported.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, impersonatedAccount.Role),
                new Claim("OriginalUserName", userName ?? string.Empty)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
            return new LoginResult
            {
                UserName = request.UserName,
                Role = impersonatedAccount.Role,
                OriginalUserName = userName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }

        public bool IsValidUserCredentials(string username, string password)
        {
            var account = GetAccount(username);
            if (account is null)
            {
                _logger.LogWarning($"No User were found with the Username [{username}]");
                return false;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, account.Salt);

            account = _accountRepository.GetAccountByUsernameAndPassword(username, hashedPassword);

            return account != null;
        }

        public LoginResult Login(LoginRequest request)
        {
            if (!IsValidUserCredentials(request.UserName, request.Password))
            {
                _logger.LogWarning($"Unauthorized");
                throw new ProblemDetailsException(StatusCodes.Status401Unauthorized, $"Unauthorized");
            }

            var account = GetAccount(request.UserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            return new LoginResult
            {
                UserName = request.UserName,
                Role = account.Role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }

        public async Task<LoginResult> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    _logger.LogWarning($"Unauthorized");
                    throw new ProblemDetailsException(StatusCodes.Status401Unauthorized, $"Unauthorized");
                }

                var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return new LoginResult
                {
                    UserName = userName,
                    Role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                };
            }
            catch (SecurityTokenException e)
            {
                _logger.LogWarning($"Unauthorized");
                throw new ProblemDetailsException(StatusCodes.Status401Unauthorized, $"Unauthorized", e);
            }
        }

        public LoginResult StopImpersonation()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity?.Name;
            var originalUserName = _httpContextAccessor.HttpContext.User.FindFirst("OriginalUserName")?.Value;
            if (string.IsNullOrWhiteSpace(originalUserName))
            {
                _logger.LogWarning($"You are not impersonating anyone.");
                throw new ProblemDetailsException(StatusCodes.Status400BadRequest, $"You are not impersonating anyone.");
            }
            _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

            var account = GetAccount(originalUserName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,originalUserName),
                new Claim(ClaimTypes.Role, account.Role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
            return new LoginResult
            {
                UserName = originalUserName,
                Role = account.Role,
                OriginalUserName = null,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }
    }
}
