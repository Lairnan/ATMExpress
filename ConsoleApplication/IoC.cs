using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using IIC.Implements;
using IIC.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

public static class IoC
{
    private static readonly IServiceProvider provider;

    public static T Resolve<T>() where T : notnull
        => provider.GetRequiredService<T>();
    
    static IoC()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILogger, Logger>()
            .AddTransient<IServer, Server>();
        
        services.AddSingleton<MenuHandler>();
        services.AddSingleton<Application>();
        services.AddMenusToService();
        
        provider = services.BuildServiceProvider();
        
        foreach (var service in services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            provider.GetRequiredService(service.ServiceType);
    }

    private static void AddMenusToService(this IServiceCollection services)
    {
        services.AddTransient<StartMenu>();
        services.AddTransient<MainMenu>();
        services.AddTransient<SettingsMenu>();
        services.AddTransient<SecurityMenu>();
    }
}