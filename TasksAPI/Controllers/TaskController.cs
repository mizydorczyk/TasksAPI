using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("/api/group/{groupId}/task")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = "JwtNotInBlacklist")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int groupId, [FromBody] CreateTaskDto dto)
        {
            var id = await _taskService.Create(groupId, dto);
            return Created($"/api/group/{groupId}/task/{id}", null);
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> Delete([FromRoute] int groupId, [FromRoute] int taskId)
        {
            await _taskService.Delete(groupId, taskId);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll([FromRoute] int groupId, [FromQuery] string isCompleted)
        {
            var result = await _taskService.Get(groupId, isCompleted);
            return Ok(result);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskDto>> Get([FromRoute] int groupId, [FromRoute] int taskId)
        {
            var result = await _taskService.GetById(groupId, taskId);
            return Ok(result);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult> Update([FromRoute] int groupId, [FromRoute] int taskId, [FromBody] UpdateTaskDto dto)
        {
            await _taskService.Update(groupId, taskId, dto);
            return Ok();
        }
    }
}
