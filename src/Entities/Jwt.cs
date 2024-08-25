namespace TasksAPI.Entities;

public class Jwt
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime? ExpDate { get; set; }
}