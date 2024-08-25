namespace TasksAPI.Models;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<TaskDto> Tasks { get; set; }
}