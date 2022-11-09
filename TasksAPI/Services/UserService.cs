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

namespace TasksAPI.Services
{
    public interface IUserService
    {
        void Register(RegisterDto dto);
        string GenerateJwt(LoginDto dto);
        void Delete(string token);
    }
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
        public void Delete(string token)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == _userContextService.GetUserId);
            if (user == null)
            {
                throw new BadRequestException("User does not exist");
            }
            _dbContext.Remove(user);
            BlacklistJwt(token);
            _dbContext.SaveChanges();
        }
        private void BlacklistJwt(string token)
        {
            JwtSecurityToken jwtToken = new(token.Substring(token.IndexOf(" ")+1));
            Jwt jwt = new()
            {
                Token = token.Substring(token.IndexOf(" ") + 1),
                ExpDate = jwtToken.ValidTo
        };
            _dbContext.Blacklist.Add(jwt);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(n => n.Email == dto.Email);
            if (user is null)
            {
                throw new ForbidException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new ForbidException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
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

        public void Register(RegisterDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
