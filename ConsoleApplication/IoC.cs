using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using CSA.Implements;
using CSA.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

public static class IoC
{
    private static readonly IServiceProvider Provider;

    public static T Resolve<T>() where T : notnull
        => Provider.GetRequiredService<T>();
    
    static IoC()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILogger, Logger>()
            .AddTransient<IServer, Server>();
        
        services.AddSingleton<MenuHandler>();
        services.AddSingleton<Application>();
        services.AddMenusToService();
        
        Provider = services.BuildServiceProvider();
        
        foreach (var service in services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            Provider.GetRequiredService(service.ServiceType);
    }

    private static void AddMenusToService(this IServiceCollection services)
    {
        services.AddTransient<StartMenu>();
        services.AddTransient<MainMenu>();
        services.AddTransient<SettingsMenu>();
        services.AddTransient<SecurityMenu>();
    }
}