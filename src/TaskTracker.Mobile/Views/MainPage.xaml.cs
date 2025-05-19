using Microsoft.Maui.Controls;
using TaskTracker.Mobile.ViewModels;
using TodoCodexPoc.Models;

namespace TaskTracker.Mobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox cb && cb.BindingContext is TodoItem item && BindingContext is MainPageViewModel vm)
        {
            vm.ToggleDoneCommand.Execute(item);
        }
    }
}
