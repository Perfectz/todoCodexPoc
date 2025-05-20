using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TodoCodexPoc.Services;

namespace TaskTracker.Mobile;

public static class MauiProgram
{
    public static MauiApp Create()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddSingleton<ITodoRepository, FileTodoRepository>();
        builder.Services.AddTransient<ViewModels.MainPageViewModel>();
        builder.Services.AddTransient<Views.MainPage>();
        builder.Services.AddTransient<Views.ArchivePage>();

        return builder.Build();
    }
}
