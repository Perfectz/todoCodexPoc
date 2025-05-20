using Microsoft.Maui.Controls;
using TaskTracker.Mobile.ViewModels;

namespace TaskTracker.Mobile.Views;

public partial class ArchivePage : ContentPage
{
    public ArchivePage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
