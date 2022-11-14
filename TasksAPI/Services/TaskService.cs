using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Entities;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public interface ITaskService
    {
        int Create(int groupId, CreateTaskDto dto);
        void Delete(int groupId, int taskId);
        List<TaskDto>Get(int groupId);
        TaskDto GetById(int groupId, int taskId);
    }
    public class TaskService : ITaskService
    {
        private readonly TasksDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public TaskService(TasksDbContext dbContext,
                           IMapper mapper,
                           IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public bool BelongsToGroup(int groupId)
        {
            var userId = _userContextService.GetUserId;
            var group = _dbContext
                .Groups
                .Include(x => x.Users)
                .FirstOrDefault(x => x.Id == groupId);
            var user = group.Users.FirstOrDefault(x => x.Id == userId);
            if(user is null)
            {
                return false;
            }
            return true;
        }
        public int Create(int groupId, CreateTaskDto dto)
        {
            var group = _dbContext
                .Groups
                .FirstOrDefault(x => x.Id == groupId);
            if(group is null)
            {
                throw new NotFoundException("Group not found");
            }
            if (!BelongsToGroup(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var task = _mapper.Map<Entities.Task>(dto);
            if(group.Tasks is null)
            {
                group.Tasks = new List<Entities.Task>();
            }
            task.CreatedDate = DateTime.Now;
            task.IsCompleted = false;
            group.Tasks.Add(task);
            _dbContext.SaveChanges();
            return task.Id;
        }

        public void Delete(int groupId, int taskId)
        {
            var group = _dbContext
                .Groups
                .Include(x => x.Tasks)
                .FirstOrDefault(x => x.Id == groupId);
            if (group is null)
            {
                throw new NotFoundException("Group not found");
            }
            if (!BelongsToGroup(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var task = group
                .Tasks
                .FirstOrDefault(x => x.Id == taskId);
            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();
        }

        public List<TaskDto> Get(int groupId)
        {
            var group = _dbContext
                .Groups
                .Include(x => x.Tasks)
                .FirstOrDefault(x => x.Id == groupId);
            if (group is null)
            {
                throw new NotFoundException("Group not found");
            }
            if (!BelongsToGroup(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var tasks = group.Tasks;
            if (tasks is null || tasks.Count == 0)
            {
                throw new NotFoundException("No tasks found");
            }

            var tasksDtos = _mapper.Map<List<TaskDto>>(tasks);
            return tasksDtos;
        }

        public TaskDto GetById(int groupId, int taskId)
        {
            var group = _dbContext
                .Groups
                .Include(x => x.Tasks)
                .FirstOrDefault(x => x.Id == groupId);
            if (group is null)
            {
                throw new NotFoundException("Group not found");
            }
            if (!BelongsToGroup(groupId))
            {
                throw new ForbidException("Insufficient permission");
            }

            var task = group.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (task is null)
            {
                throw new NotFoundException("Task not found");
            }

            var taskDto = _mapper.Map<TaskDto>(task);
            return taskDto;
        }
    }
}
