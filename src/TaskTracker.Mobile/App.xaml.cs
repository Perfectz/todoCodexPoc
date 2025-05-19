using Microsoft.Maui.Controls;
using TaskTracker.Mobile.Views;

namespace TaskTracker.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
