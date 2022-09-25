using Formuler.Shared.DTO.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Business.Services;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var username = User.Identity.Name;
            return Ok(await _userService.GetUser(username));
        }

        [AllowAnonymous]
        [HttpPost("singup")]
        public async Task<ActionResult> Singup(RegisterUserRequestDTO registerUserRequest)
        {
            return Ok(await _userService.RegisterUser(registerUserRequest));
        }
    }
}
