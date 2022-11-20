using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterDto dto)
        {
            await _userService.Register(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]LoginDto dto)
        {
            var token = await _userService.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpDelete]
        [Authorize][Authorize(Policy = "JwtNotInBlacklist")]
        public async Task<ActionResult> Delete([FromHeader]string Authorization)
        {
            await _userService.Delete(Authorization);
            return NoContent();
        }

        [HttpPut]
        [Authorize][Authorize(Policy = "JwtNotInBlacklist")]
        public async Task<ActionResult> ChangePassword([FromHeader]string Authorization, [FromBody]ChangePasswordDto dto)
        {
            await _userService.ChangePassword(dto, Authorization);
            return Ok();
        }
    }
}
