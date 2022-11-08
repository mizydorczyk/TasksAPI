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
        public ActionResult Register([FromBody]RegisterDto dto)
        {
            _userService.Register(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            var token = _userService.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpDelete("delete")]
        [Authorize]
        public ActionResult Delete()
        {
            _userService.Delete();
            return NoContent();
        }
    }
}
