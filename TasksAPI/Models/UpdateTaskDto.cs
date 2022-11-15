namespace TasksAPI.Models
{
    public class UpdateTaskDto
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool? IsCompleted { get; set; }
    }
}