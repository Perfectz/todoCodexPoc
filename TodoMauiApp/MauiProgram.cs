using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System.Diagnostics;
using TodoMauiApp.Data;
using TodoMauiApp.Services;
using TodoMauiApp.ViewModels;

namespace TodoMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton(provider => 
        {
            try
            {
                return provider.GetRequiredService<DatabaseService>().GetContext();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting DB context: {ex.Message}");
                return null;
            }
        });
        builder.Services.AddSingleton<TodoRepository>();
        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddSingleton<MainPage>();

        var app = builder.Build();
        ServiceProvider.Initialize(app.Services);
        
        // Initialize the database
        try
        {
            var dbService = app.Services.GetRequiredService<DatabaseService>();
            var success = dbService.Initialize();
            if (!success)
            {
                Debug.WriteLine("Failed to initialize database. App might be unstable.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Critical error during initialization: {ex.Message}");
        }
        
        return app;
    }
}

// Global service provider for accessing DI services
public static class ServiceProvider
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static T? GetService<T>() where T : class
    {
        if (_serviceProvider == null)
        {
            Debug.WriteLine("WARNING: ServiceProvider not initialized");
            return null;
        }
        return _serviceProvider.GetService<T>();
    }
}
