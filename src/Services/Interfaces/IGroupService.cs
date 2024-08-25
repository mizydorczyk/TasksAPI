using TasksAPI.Models;

namespace Services.Interfaces;

public interface IGroupService
{
    Task<int> Create(CreateGroupDto dto);
    Task Join(string invitationCode);
    Task<List<GroupDto>> Get();
    Task Delete(int groupId);
    Task<string> GetInvitationCode(int groupId);
    Task RenewInvitationCode(int groupId);
}