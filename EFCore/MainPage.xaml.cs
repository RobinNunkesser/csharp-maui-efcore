namespace EFCore;

public partial class MainPage : ContentPage
{
    private readonly TodoContext _db;

    public MainPage()
    {
        InitializeComponent();
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todos.db3");
        _db = new TodoContext(dbPath);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListAsync();
    }

    private async Task RefreshListAsync()
    {
        TodoList.ItemsSource = await Task.Run(() => _db.Todos.ToList());
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        var title = TitleEntry.Text?.Trim();
        if (string.IsNullOrEmpty(title)) return;

        _db.Todos.Add(new TodoItem { Title = title });
        await _db.SaveChangesAsync();
        TitleEntry.Text = string.Empty;
        await RefreshListAsync();
    }

    private async void OnDoneChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox cb && cb.BindingContext is TodoItem item)
        {
            item.Done = e.Value;
            await _db.SaveChangesAsync();
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is TodoItem item)
        {
            _db.Todos.Remove(item);
            await _db.SaveChangesAsync();
            await RefreshListAsync();
        }
    }
}
