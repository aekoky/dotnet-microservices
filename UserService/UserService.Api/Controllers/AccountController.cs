using System.Security.Claims;
using System.Threading.Tasks;
using Core.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Business.DTO.Requests;
using UserService.Business.DTO.Results;
using UserService.Business.Services.Account;
using UserService.Business.Services.User;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _accountService = accountService;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_accountService.Login(request));
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                UserName = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                OriginalUserName = User.FindFirst("OriginalUserName")?.Value
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity?.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _accountService.RefreshToken(request));
        }

        [HttpPost("impersonation")]
        [Authorize(Roles = UserRoles.Admin)]
        public ActionResult Impersonate([FromBody] ImpersonationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_accountService.Impersonate(request));
        }

        [HttpPost("stop-impersonation")]
        public ActionResult StopImpersonation()
        {
            return Ok(_accountService.StopImpersonation());
        }

        [AllowAnonymous]
        [HttpPost("singup")]
        public ActionResult Singup(RegisterUserRequest registerUserRequest)
        {
            return Ok(_userService.RegisterUser(registerUserRequest));
        }
    }
}
