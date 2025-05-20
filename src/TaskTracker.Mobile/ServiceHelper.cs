using System;
using Microsoft.Extensions.DependencyInjection;

namespace TaskTracker.Mobile;

public static class ServiceHelper
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider provider)
    {
        _serviceProvider = provider;
    }

    public static T GetService<T>() where T : notnull
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("Service provider not initialized");
        return _serviceProvider.GetRequiredService<T>();
    }
}
