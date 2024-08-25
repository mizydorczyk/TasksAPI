using TasksAPI.Models;
using AutoMapper;
using TasksAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using TasksAPI.Exceptions;
using Services.Interfaces;

namespace TasksAPI.Services;

public class UserService : IUserService
{
    private readonly TasksDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IUserContextService _userContextService;

    public UserService(TasksDbContext context,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings,
        IUserContextService userContextService)
    {
        _dbContext = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _userContextService = userContextService;
    }

    public async System.Threading.Tasks.Task Delete(string token)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == _userContextService.GetUserId) ?? throw new NotFoundException("User does not exist");

        _dbContext.Remove(user);
        await BlacklistJwt(token);
        await _dbContext.SaveChangesAsync();
    }

    private async System.Threading.Tasks.Task BlacklistJwt(string token)
    {
        JwtSecurityToken jwtToken = new(token.Substring(token.IndexOf(" ") + 1));
        Jwt jwt = new()
        {
            Token = token.Substring(token.IndexOf(" ") + 1),
            ExpDate = jwtToken.ValidTo
        };
        _dbContext.Blacklist.Add(jwt);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GenerateJwt(LoginDto dto)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(n => n.Email == dto.Email) ?? throw new ForbidException("Invalid username or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) throw new ForbidException("Invalid username or password");

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public async System.Threading.Tasks.Task Register(RegisterDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
        user.PasswordHash = hashedPassword;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task ChangePassword(ChangePasswordDto dto, string token)
    {
        var userId = _userContextService.GetUserId;
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId) ?? throw new NotFoundException("Something went wrong");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
        if (result == PasswordVerificationResult.Failed) throw new ForbidException("Invalid old password");

        var hashedPassword = _passwordHasher.HashPassword(user, dto.NewPassword);
        user.PasswordHash = hashedPassword;

        await BlacklistJwt(token);
        await _dbContext.SaveChangesAsync();
    }
}