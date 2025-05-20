using Microsoft.Maui.Controls;
using System.Threading.Tasks;
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
            item.IsDone = e.Value;
            vm.ToggleDoneCommand.Execute(item);
        }
    }

    // Removed invalid item animation handler that assumed ViewCell containers
}
