﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using TasksAPI.Entities;
using TasksAPI.Exceptions;
using TasksAPI.Models;

namespace TasksAPI.Services;

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

    private async Task<bool> BelongsToGroup(int groupId)
    {
        var userId = _userContextService.GetUserId;
        var group = await _dbContext
            .Groups
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == groupId);

        var user = group.Users.FirstOrDefault(x => x.Id == userId);

        return user is not null;
    }

    public async Task<int> Create(int groupId, CreateTaskDto dto)
    {
        var group = await _dbContext
            .Groups
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new NotFoundException("Group not found");

        if (!await BelongsToGroup(groupId)) throw new ForbidException("Insufficient permission");

        var task = _mapper.Map<Entities.Task>(dto);
        task.CreatedDate = DateTime.Now;
        task.IsCompleted = false;
        group.Tasks.Add(task);

        await _dbContext.SaveChangesAsync();
        return task.Id;
    }

    public async System.Threading.Tasks.Task Delete(int groupId, int taskId)
    {
        var group = await _dbContext
            .Groups
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new NotFoundException("Group not found");

        if (!await BelongsToGroup(groupId)) throw new ForbidException("Insufficient permission");

        var task = group
            .Tasks
            .FirstOrDefault(x => x.Id == taskId) ?? throw new NotFoundException("Task not found");

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<TaskDto>> Get(int groupId, string filter)
    {
        var group = await _dbContext
            .Groups
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new NotFoundException("Group not found");

        if (!await BelongsToGroup(groupId)) throw new ForbidException("Insufficient permission");

        var tasks = group.Tasks.Where(x => filter is null || string.Equals(x.IsCompleted.ToString(), filter, StringComparison.InvariantCultureIgnoreCase));
        if (tasks is null || !tasks.Any()) throw new NotFoundException("No tasks found");

        var tasksDtos = _mapper.Map<List<TaskDto>>(tasks);
        return tasksDtos;
    }

    public async Task<TaskDto> GetById(int groupId, int taskId)
    {
        var group = await _dbContext
            .Groups
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new NotFoundException("Group not found");

        if (!await BelongsToGroup(groupId)) throw new ForbidException("Insufficient permission");

        var task = group.Tasks.FirstOrDefault(x => x.Id == taskId) ?? throw new NotFoundException("Task not found");

        var taskDto = _mapper.Map<TaskDto>(task);
        return taskDto;
    }

    public async System.Threading.Tasks.Task Update(int groupId, int taskId, UpdateTaskDto dto)
    {
        var group = await _dbContext
            .Groups
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == groupId) ?? throw new NotFoundException("Group not found");

        if (!await BelongsToGroup(groupId)) throw new ForbidException("Insufficient permission");

        var task = group
            .Tasks
            .FirstOrDefault(x => x.Id == taskId) ?? throw new NotFoundException("Task not found");

        if (dto.Title is not null) task.Title = dto.Title;

        if (dto.Description is not null) task.Description = dto.Description;

        if (dto.Deadline is not null) task.Deadline = dto.Deadline;

        if (dto.IsCompleted is not null) task.IsCompleted = (bool)dto.IsCompleted;

        _dbContext.Update(task);
        await _dbContext.SaveChangesAsync();
    }
}