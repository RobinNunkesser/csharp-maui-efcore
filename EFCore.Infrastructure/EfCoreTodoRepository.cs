using Microsoft.EntityFrameworkCore;
using TodoEF.Core;

namespace TodoEF.Infrastructure;

internal class TodoDbContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<TodoItem> Todos { get; set; } = null!;

    public TodoDbContext(string dbPath)
    {
        _dbPath = dbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_dbPath}");
}

public class EfCoreTodoRepository : ITodoRepository
{
    private readonly string _dbPath;

    public EfCoreTodoRepository(string dbPath)
    {
        _dbPath = dbPath;
        using var ctx = CreateContext();
        ctx.Database.EnsureCreated();
    }

    private TodoDbContext CreateContext() => new(_dbPath);

    public async Task<List<TodoItem>> GetAllAsync()
    {
        await using var ctx = CreateContext();
        return await ctx.Todos.ToListAsync();
    }

    public async Task SaveAsync(TodoItem item)
    {
        await using var ctx = CreateContext();
        if (item.Id == 0)
            ctx.Todos.Add(item);
        else
            ctx.Todos.Update(item);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem item)
    {
        await using var ctx = CreateContext();
        var entity = await ctx.Todos.FindAsync(item.Id);
        if (entity is not null)
        {
            ctx.Todos.Remove(entity);
            await ctx.SaveChangesAsync();
        }
    }
}
