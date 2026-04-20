using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoEF.Core;

public partial class TodoItem : ObservableObject
{
    public int Id { get; set; }

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private bool _done;
}
