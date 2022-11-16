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
        public ActionResult<string> CreateGroup([FromBody] CreateGroupDto dto)
        {
            var id = _groupService.Create(dto);
            return Created($"/api/group/{id}", null);
        }

        [HttpPost("join ")]
        public ActionResult Join([FromHeader]string invitationCode)
        {
            _groupService.Join(invitationCode);
            return Ok();
        }

        [HttpGet("{groupId}/getInvitationCode")]
        public ActionResult<string> GetInvitationCode([FromRoute]int groupId)
        {
            var result = _groupService.GetInvitationCode(groupId);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetAll()
        {
            var groupsDtos = _groupService.Get();
            return Ok(groupsDtos);
        }

        [HttpDelete("{groupId}")]
        public ActionResult Delete([FromRoute]int groupId)
        {
            _groupService.Delete(groupId);
            return NoContent();
        }
    }
}
