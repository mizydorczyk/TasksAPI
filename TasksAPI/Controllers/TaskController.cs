using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("/api/group/{groupId}/task")]
    [ApiController]
    [Authorize][Authorize(Policy = "JwtNotInBlacklist")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public ActionResult Create([FromRoute]int groupId, [FromBody]CreateTaskDto dto)
        {
            var id = _taskService.Create(groupId, dto);
            return Created($"/api/group/{groupId}/task/{id}", null);
        }

        [HttpDelete("{taskId}")]
        public ActionResult Delete([FromRoute]int groupId, [FromRoute]int taskId)
        {
            _taskService.Delete(groupId, taskId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskDto>> GetAll([FromRoute]int groupId)
        {
            var result = _taskService.Get(groupId);
            return Ok(result);
        }

        [HttpGet("{taskId}")]
        public ActionResult<TaskDto> Get([FromRoute]int groupId, [FromRoute]int taskId)
        {
            var result = _taskService.GetById(groupId, taskId);
            return Ok(result);
        }
    }
}
