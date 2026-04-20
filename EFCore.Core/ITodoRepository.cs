namespace TodoEF.Core;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync();
    Task SaveAsync(TodoItem item);
    Task DeleteAsync(TodoItem item);
}
