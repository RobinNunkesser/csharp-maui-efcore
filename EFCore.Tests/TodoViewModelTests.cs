using TodoEF.Core;
using TodoEF.Core.ViewModels;

namespace EFCore.Tests;

public class TodoViewModelTests
{
    [Fact]
    public async Task AddAsync_AddsItemToTodos()
    {
        var repo = new FakeTodoRepository();
        var vm = new TodoViewModel(repo);
        vm.NewTitle = "Test";

        await vm.AddAsync();

        Assert.Single(vm.Todos);
        Assert.Equal("Test", vm.Todos[0].Title);
    }

    [Fact]
    public async Task AddAsync_EmptyTitle_DoesNotAdd()
    {
        var repo = new FakeTodoRepository();
        var vm = new TodoViewModel(repo);
        vm.NewTitle = "   ";

        await vm.AddAsync();

        Assert.Empty(vm.Todos);
    }

    [Fact]
    public async Task DeleteAsync_RemovesItemFromTodos()
    {
        var repo = new FakeTodoRepository();
        await repo.SaveAsync(new TodoItem { Title = "Test" });
        var vm = new TodoViewModel(repo);
        await vm.LoadAsync();

        await vm.DeleteAsync(vm.Todos[0]);

        Assert.Empty(vm.Todos);
    }

    [Fact]
    public async Task ToggleDoneAsync_SetsDoneTrue()
    {
        var repo = new FakeTodoRepository();
        await repo.SaveAsync(new TodoItem { Title = "Test" });
        var vm = new TodoViewModel(repo);
        await vm.LoadAsync();

        await vm.ToggleDoneAsync(vm.Todos[0]);

        Assert.True(vm.Todos[0].Done);
    }

    [Fact]
    public async Task ToggleDoneAsync_TogglesBackToFalse()
    {
        var repo = new FakeTodoRepository();
        var item = new TodoItem { Title = "Test", Done = true };
        await repo.SaveAsync(item);
        var vm = new TodoViewModel(repo);
        await vm.LoadAsync();

        await vm.ToggleDoneAsync(vm.Todos[0]);

        Assert.False(vm.Todos[0].Done);
    }
}

internal class FakeTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _items = [];
    private int _nextId = 1;

    public Task<List<TodoItem>> GetAllAsync() =>
        Task.FromResult(_items.ToList());

    public Task SaveAsync(TodoItem item)
    {
        if (item.Id == 0)
        {
            item.Id = _nextId++;
            _items.Add(item);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TodoItem item)
    {
        _items.RemoveAll(i => i.Id == item.Id);
        return Task.CompletedTask;
    }
}
