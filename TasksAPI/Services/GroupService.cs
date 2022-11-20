using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Entities;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public interface IGroupService
    {
        Task<int> Create(CreateGroupDto dto);
        System.Threading.Tasks.Task Join(string invitationCode);
        Task<List<GroupDto>> Get();
        System.Threading.Tasks.Task Delete(int groupId);
        Task<string> GetInvitationCode(int groupId);
    }
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly TasksDbContext _dbContext;
        private readonly AuthenticationSettings _authenticationSettings;

        public GroupService(IMapper mapper,
                            IUserContextService userContextService,
                            TasksDbContext dbContext,
                            AuthenticationSettings authenticationSettings)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _dbContext = dbContext;
            _authenticationSettings = authenticationSettings;
        }
        private async Task<bool> IsGroupOwner(int groupId)
        {
            var userId = _userContextService.GetUserId;
            var group = await _dbContext
                .Groups
                .FirstOrDefaultAsync(x => x.Id == groupId);
            if(group.CreatedById == userId)
            {
                return true;
            }
            return false;
        }
        public async Task<int> Create(CreateGroupDto dto)
        {
            var group = _mapper.Map<Group>(dto);
            group.CreatedById = (int)_userContextService.GetUserId;

            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == _userContextService.GetUserId);
            if (user is null)
            {
                throw new BadRequestException("Something went wrong");
            }

            if(group.Users is null)
            {
                group.Users = new List<User>();
            }
            if (user.Groups is null)
            {
                user.Groups = new List<Group>();
            }
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
                .FirstOrDefaultAsync(x => x.Id == groupId);
            if (group is null)
            {
                throw new BadRequestException("Group does not exist");
            }
            if (!await IsGroupOwner(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

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
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                throw new BadRequestException("Something went wrong");
            }

            var groups = user.Groups;
            var groupsDtos = _mapper.Map<List<GroupDto>>(groups);
            return groupsDtos;
        }

        public async System.Threading.Tasks.Task Join(string invitationCode)
        {
            int groupId;
            try
            {
                groupId = Convert.ToInt32
                    (Crypto.DecryptString(invitationCode, _authenticationSettings.InvitationCodeKey));
            }
            catch
            {
                throw new BadRequestException("Group does not exist");
            }

            var group = await _dbContext
                .Groups
                .FirstOrDefaultAsync(x => x.Id == groupId);
            if (group is null)
            {
                throw new BadRequestException("Group does not exist");
            }

            var userId = _userContextService.GetUserId;
            var user = await _dbContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == userId);
            if(user is null)
            {
                throw new NotFoundException("User does not exist");
            }

            if (group.Users == null)
            {
                group.Users = new List<User>();
            }
            if (user.Groups == null)
            {
                user.Groups = new List<Group>();
            }

            group.Users.Add(user);
            user.Groups.Add(group);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<string> GetInvitationCode(int groupId)
        {
            var group = await _dbContext
                .Groups
                .FirstOrDefaultAsync(x => x.Id == groupId);
            if (group is null)
            {
                throw new BadRequestException("Group does not exist");
            }
            if (!await IsGroupOwner(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var invitationCode = Crypto.EncryptString(groupId.ToString(), _authenticationSettings.InvitationCodeKey);
            return invitationCode;
        }        
    }
}