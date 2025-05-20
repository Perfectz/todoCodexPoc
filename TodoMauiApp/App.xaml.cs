using Microsoft.Maui.Controls;
using TodoMauiApp.Services;

namespace TodoMauiApp;

public partial class App : Application
{
    public App(DatabaseService databaseService)
    {
        InitializeComponent();
        
        // Ensure database is initialized
        databaseService.Initialize();
        
        MainPage = ServiceProvider.GetService<MainPage>();
    }
}
