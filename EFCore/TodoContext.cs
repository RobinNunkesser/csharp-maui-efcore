using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class TodoContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<TodoItem> Todos { get; set; } = null!;

    public TodoContext(string dbPath)
    {
        _dbPath = dbPath;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");
}
