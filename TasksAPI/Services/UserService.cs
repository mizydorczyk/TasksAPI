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
    }
    public class UserService : IUserService
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserService(TasksDbContext context, 
                           IMapper mapper,
                           IPasswordHasher<User> passwordHasher,
                           AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _context
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
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
