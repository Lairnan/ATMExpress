using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using CSA.Implements;
using CSA.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

internal static class Program
{
    private static IServiceProvider? _provider;
    private static IServiceProvider Provider
    {
        get
        {
            if(_provider == null) InitializeDependency();
            return _provider!;
        }
    }
    
    public static async Task Main(string[] args)
    {
        IMenu? result = new StartMenu();
        var app = new Application(Provider);
        do
        {
            result = app.Menu(result);
            Thread.Sleep(250);
            Console.Clear();
        } while (result != null);
        
        Console.Write("Press any key to continue");
        Console.ReadKey();

        Console.Write("\nClosing application...");
        await Task.Delay(1000);
    }

    private static void InitializeDependency()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILogger, Logger>()
            .AddTransient<IServer, Server>();
        
        services.AddSingleton<MenuHandler>();
        services.AddMenusToService();
        
        _provider = services.BuildServiceProvider();

        foreach (var service in services.Where(s => s.Lifetime == ServiceLifetime.Singleton))
            Provider.GetRequiredService(service.GetType());
    }

    private static void AddMenusToService(this IServiceCollection services)
    {
        services.AddTransient<StartMenu>();
        services.AddTransient<MainMenu>();
        services.AddTransient<SettingsMenu>();
        services.AddTransient<SecurityMenu>();
    }
}