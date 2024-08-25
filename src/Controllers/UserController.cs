using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Controllers;

[Route("/api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        await _userService.Register(dto);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
    {
        var token = await _userService.GenerateJwt(dto);
        return Ok(token);
    }

    [HttpDelete]
    [Authorize]
    [Authorize(Policy = "JwtNotInBlacklist")]
    public async Task<ActionResult> Delete()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) throw new BadRequestException("Something went wrong");

        var authorization = (string)httpContext.Request.Headers.Authorization;

        await _userService.Delete(authorization);
        return NoContent();
    }

    [HttpPatch]
    [Authorize]
    [Authorize(Policy = "JwtNotInBlacklist")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) throw new BadRequestException("Something went wrong");

        var authorization = (string)httpContext.Request.Headers.Authorization;

        await _userService.ChangePassword(dto, authorization);
        return Ok();
    }
}