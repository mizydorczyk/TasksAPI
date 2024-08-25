using System.Security.Claims;

namespace Services.Interfaces;

public interface IUserContextService
{
    int? GetUserId { get; }
    ClaimsPrincipal User { get; }
}