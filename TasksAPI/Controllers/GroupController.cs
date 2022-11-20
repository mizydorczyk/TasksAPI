using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Entities;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("/api/group")]
    [ApiController]
    [Authorize][Authorize(Policy = "JwtNotInBlacklist")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var id = await _groupService.Create(dto);
            return Created($"/api/group/{id}", null);
        }

        [HttpPost("join")]
        public async Task<ActionResult> Join([FromHeader]string invitationCode)
        {
            await _groupService.Join(invitationCode);
            return Ok();
        }

        [HttpGet("{groupId}/getInvitationCode")]
        public async Task<ActionResult<string>> GetInvitationCode([FromRoute]int groupId)
        {
            var result = await _groupService.GetInvitationCode(groupId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAll()
        {
            var groupsDtos = await _groupService.Get();
            return Ok(groupsDtos);
        }

        [HttpDelete("{groupId}")]
        public async Task<ActionResult> Delete([FromRoute]int groupId)
        {
            await _groupService.Delete(groupId);
            return NoContent();
        }
    }
}
