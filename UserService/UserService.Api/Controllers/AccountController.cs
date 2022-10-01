using System.Threading.Tasks;
using Formuler.Shared.DTO.UserService;
using Formuler.WebCore.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Business.Services;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _accountService = accountService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _accountService.Login(request));
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
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _accountService.RefreshToken(request));
        }

        [HttpPost("impersonation")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Impersonate([FromBody] ImpersonationRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _accountService.Impersonate(request));
        }

        [HttpPost("stop-impersonation")]
        public async Task<ActionResult> StopImpersonation()
        {
            return Ok(await _accountService.StopImpersonation());
        }
    }
}
