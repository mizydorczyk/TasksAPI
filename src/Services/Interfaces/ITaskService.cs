using TasksAPI.Models;

namespace Services.Interfaces;

public interface ITaskService
{
    Task<int> Create(int groupId, CreateTaskDto dto);
    Task Delete(int groupId, int taskId);
    Task<List<TaskDto>> Get(int groupId, string filter);
    Task<TaskDto> GetById(int groupId, int taskId);
    Task Update(int groupId, int taskId, UpdateTaskDto dto);
}