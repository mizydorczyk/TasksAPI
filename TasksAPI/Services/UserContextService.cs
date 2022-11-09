using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace TasksAPI.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? GetUserId => User is null ? null :
            (int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value));
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    }
}
