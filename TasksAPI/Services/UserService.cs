using TasksAPI.Models;
using AutoMapper;
using TasksAPI.Entities;
using Microsoft.AspNetCore.Identity;

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

        public UserService(TasksDbContext context, 
                           IMapper mapper,
                           IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public string GenerateJwt(LoginDto dto)
        {
            throw new NotImplementedException();
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
