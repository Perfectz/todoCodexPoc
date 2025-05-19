using Microsoft.Maui.Controls;
namespace TodoMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new MainPage();
    }
}
