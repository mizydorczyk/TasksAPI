using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using TasksAPI.Entities;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Services;

public class GroupService : IGroupService
{
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly TasksDbContext _dbContext;

    public GroupService(IMapper mapper,
        IUserContextService userContextService,
        TasksDbContext dbContext)
    {
        _mapper = mapper;
        _userContextService = userContextService;
        _dbContext = dbContext;
    }

    public async Task<string> GetUniqueInvitationCode(int range = 10)
    {
        var invitationCodes = await _dbContext
            .Groups
            .Select(x => x.InvitationCode)
            .ToListAsync();

        var chars = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var isUnique = false;
        string result;
        do
        {
            var random = new Random();
            result = new string(
                Enumerable.Repeat(chars, range)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            if (!invitationCodes.Contains(result)) isUnique = true;
        } while (!isUnique);

        return result;
    }

    private async Task<bool> IsGroupOwner(int groupId)
    {
        var userId = _userContextService.GetUserId;
        var group = await _dbContext
            .Groups
            .FirstOrDefaultAsync(x => x.Id == groupId);
        if (group.CreatedById == userId) return true;

        return false;
    }

    public async Task<int> Create(CreateGroupDto dto)
    {
        var group = _mapper.Map<Group>(dto);
        group.CreatedById = (int)_userContextService.GetUserId;
        group.InvitationCode = await GetUniqueInvitationCode();

        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == _userContextService.GetUserId) ?? throw new BadRequestException("Something went wrong");

        group.Users.Add(user);
        user.Groups.Add(group);

        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
        return group.Id;
    }

    public async System.Threading.Tasks.Task Delete(int groupId)
    {
        var group = await _dbContext
            .Groups
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new BadRequestException("Group does not exist");

        if (!await IsGroupOwner(groupId)) throw new ForbidException("Insufficient permission");

        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<GroupDto>> Get()
    {
        var userId = _userContextService.GetUserId;
        var user = await _dbContext
            .Users
            .Include(x => x.Groups)
            .ThenInclude(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == userId) ?? throw new BadRequestException("Something went wrong");

        var groups = user.Groups;
        var groupsDtos = _mapper.Map<List<GroupDto>>(groups);
        return groupsDtos;
    }

    public async System.Threading.Tasks.Task Join(string invitationCode)
    {
        var group = await _dbContext
                        .Groups
                        .FirstOrDefaultAsync(x => x.InvitationCode == invitationCode) ??
                    throw new BadRequestException("Invitation code is invalid or has already expired");

        var userId = _userContextService.GetUserId;
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) throw new NotFoundException("User does not exist");

        group.Users.Add(user);
        user.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GetInvitationCode(int groupId)
    {
        var group = await _dbContext
            .Groups
            .FirstOrDefaultAsync(x => x.Id == groupId);
        if (group is null) throw new BadRequestException("Group does not exist");

        if (!await IsGroupOwner(groupId)) throw new ForbidException("Insufficient permission");

        var invitationCode = group.InvitationCode;
        return invitationCode;
    }

    public async System.Threading.Tasks.Task RenewInvitationCode(int groupId)
    {
        var group = await _dbContext
            .Groups
            .FirstOrDefaultAsync(x => x.Id == groupId);
        if (group is null) throw new BadRequestException("Group does not exist");

        if (!await IsGroupOwner(groupId)) throw new ForbidException("Insufficient permission");

        group.InvitationCode = await GetUniqueInvitationCode();
        await _dbContext.SaveChangesAsync();
    }
}