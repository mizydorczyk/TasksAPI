using Microsoft.EntityFrameworkCore;

namespace TasksAPI.Entities;

public class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Jwt> Blacklist { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<Jwt>()
            .Property(u => u.Token)
            .IsRequired();

        modelBuilder.Entity<Jwt>()
            .Property(u => u.ExpDate)
            .IsRequired();

        modelBuilder.Entity<Group>()
            .Property(u => u.Name)
            .IsRequired();

        modelBuilder.Entity<Task>()
            .Property(u => u.Title)
            .IsRequired();
    }
}