﻿namespace TasksAPI.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatedById { get; set; }
    public string InvitationCode { get; set; }
    public virtual List<User> Users { get; set; } = new();
    public virtual List<Task> Tasks { get; set; } = new();
}