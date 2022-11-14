using AutoMapper;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Entities;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public interface IGroupService
    {
        int Create(CreateGroupDto dto);
        void Invite(int groupId, int userId);
        List<GroupDto> Get();
        void Delete(int groupId);
    }
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
        private bool IsGroupOwner(int groupId)
        {
            var userId = _userContextService.GetUserId;
            var group = _dbContext
                .Groups
                .FirstOrDefault(x => x.Id == groupId);
            if(group.CreatedById == userId)
            {
                return true;
            }
            return false;
        }
        public int Create(CreateGroupDto dto)
        {
            var group = _mapper.Map<Group>(dto);
            group.CreatedById = (int)_userContextService.GetUserId;

            var user = _dbContext
                .Users
                .FirstOrDefault(x => x.Id == _userContextService.GetUserId);
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
            _dbContext.SaveChanges();
            return group.Id;
        }

        public void Delete(int groupId)
        {
            var group = _dbContext
                .Groups
                .FirstOrDefault(x => x.Id == groupId);
            if (group is null)
            {
                throw new BadRequestException("Group does not exist");
            }
            if (!IsGroupOwner(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            _dbContext.Groups.Remove(group);
            _dbContext.SaveChanges();
        }

        public List<GroupDto> Get() 
        {
            var userId = _userContextService.GetUserId;
            var user = _dbContext
                .Users
                .Include(x => x.Groups)
                .ThenInclude(x => x.Tasks)
                .FirstOrDefault(x => x.Id == userId);
            if (user is null)
            {
                throw new BadRequestException("Something went wrong");
            }

            var groups = user.Groups;
            var groupsDtos = _mapper.Map<List<GroupDto>>(groups);
            return groupsDtos;
        }

        public void Invite(int groupId, int userId)
        {
            var group = _dbContext
                .Groups
                .FirstOrDefault(x => x.Id == groupId);
            if(group is null)
            {
                throw new NotFoundException("Group does not exist");
            }
            if (!IsGroupOwner(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
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
            _dbContext.SaveChanges();
        }
    }
}