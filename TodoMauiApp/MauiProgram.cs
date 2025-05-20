using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
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
        builder.Services.AddSingleton(provider => provider.GetRequiredService<DatabaseService>().GetContext());
        builder.Services.AddSingleton<TodoRepository>();
        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddSingleton<MainPage>();

        var app = builder.Build();
        ServiceProvider.Initialize(app.Services);
        return app;
    }
}

// Global service provider for accessing DI services
public static class ServiceProvider
{
    private static IServiceProvider _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetService<T>();
    }
}
