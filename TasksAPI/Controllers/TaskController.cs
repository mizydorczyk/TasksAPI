using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("/api/group/{groupId}/")]
    [ApiController]
    [Authorize][Authorize(Policy = "JwtNotInBlacklist")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
    }
}
