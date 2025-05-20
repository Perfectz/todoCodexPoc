using Microsoft.Maui.Controls;
using TaskTracker.Mobile.ViewModels;
using TaskTracker.Mobile;

namespace TaskTracker.Mobile.Views;

public partial class ArchivePage : ContentPage
{
    public ArchivePage() : this(ServiceHelper.GetService<MainPageViewModel>())
    {
    }

    public ArchivePage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
