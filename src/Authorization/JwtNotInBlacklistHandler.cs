using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using TasksAPI.Entities;
using System.Text;

namespace TasksAPI.Authorization;

public class JwtNotInBlacklistHandler : AuthorizationHandler<JwtNotInBlacklist>
{
    private readonly TasksDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtNotInBlacklistHandler(TasksDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    private static string GetJwtToken(HttpContext httpContext)
    {
        var token = "";
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var s) && s.Any())
        {
            token = s.First();
            token = token.Substring(token.IndexOf(" ", StringComparison.Ordinal) + 1);
        }

        return token;
    }

    protected override System.Threading.Tasks.Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtNotInBlacklist requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            context.Fail();

        var result = _dbContext.Blacklist.FirstOrDefault(x => x.Token == GetJwtToken(httpContext));
        if (result == null)
            context.Succeed(requirement);
        else
            context.Fail();

        return System.Threading.Tasks.Task.CompletedTask;
    }
}