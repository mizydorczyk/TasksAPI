using TasksAPI.Models;

namespace Services.Interfaces;

public interface IUserService
{
    Task Register(RegisterDto dto);
    Task<string> GenerateJwt(LoginDto dto);
    Task Delete(string token);
    Task ChangePassword(ChangePasswordDto dto, string token);
}