using Microsoft.EntityFrameworkCore;
using TodoMauiApp.Models;

namespace TodoMauiApp.Data;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}
