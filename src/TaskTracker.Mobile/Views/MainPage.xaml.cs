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
        TaskList.ItemAppearing += OnItemAppearing;
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox cb && cb.BindingContext is TodoItem item && BindingContext is MainPageViewModel vm)
        {
            vm.ToggleDoneCommand.Execute(item);
        }
    }

    private async void OnItemAppearing(object? sender, ItemVisibilityEventArgs e)
    {
        if (e.Item is ViewCell v)
        {
            v.Opacity = 0;
            v.TranslationX = 20;
            await Task.WhenAll(
                v.FadeTo(1, 200, Easing.CubicOut),
                v.TranslateTo(0, 0, 200, Easing.CubicOut));
        }
    }
}
