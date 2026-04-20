using TodoEF.Core.ViewModels;

namespace EFCore;

public partial class MainPage : ContentPage
{
    public MainPage(TodoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TodoViewModel vm)
            vm.LoadCommand.Execute(null);
    }
}
