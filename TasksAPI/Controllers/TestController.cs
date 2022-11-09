using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TasksAPI.Controllers
{
    public class TestController : ControllerBase
    {
        [Route("/api/test")]

        [HttpGet]
        [Authorize(Policy = "JwtNotInBlacklist")]
        public ActionResult<string> Get()
        {
            return Ok("test");
        }
    }
}
