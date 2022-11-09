using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using TasksAPI.Entities;
using System.Text;

namespace TasksAPI.Authorization
{
    public class JwtNotInBlacklistHandler : AuthorizationHandler<JwtNotInBlacklist>
    {
        private readonly TasksDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtNotInBlacklistHandler(TasksDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        private string GetJwtToken()
        {
            string token = "";
            if (_httpContextAccessor
                .HttpContext
                .Request
                .Headers
                .TryGetValue("Authorization", out StringValues s) && s.Any())
            {
                token = s.First();
                token = token.Substring(token.IndexOf(" ") + 1);
            }
            return token;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtNotInBlacklist requirement)
        {
            var result = _dbContext.Blacklist.FirstOrDefault(x => x.Token == GetJwtToken());
            if (result == null)
            {
                context.Succeed(requirement);

            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
