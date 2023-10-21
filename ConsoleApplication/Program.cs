using ConsoleApplication.Globalization;
using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using CSA.Implements;
using CSA.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

internal static class Program
{
    private static IServiceProvider? _provider;
    public static IServiceProvider Provider
    {
        get
        {
            if(_provider == null) InitializeDependency();
            return _provider!;
        }
    }
    
    public static async Task Main(string[] args)
    {
        args.HandleArguments();
        
        IMenu? result = new StartMenu();
        var app = new Application(Provider);
        do
        {
            result = app.Menu(result);
            Thread.Sleep(250);
            Console.CursorTop = 0;
            Console.Clear();
        } while (result != null);

        Console.Write($"\n{Translate.GetString("CloseApp")}");
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
    }

    private static void AddMenusToService(this IServiceCollection services)
    {
        services.AddTransient<StartMenu>();
        services.AddTransient<MainMenu>();
        services.AddTransient<SettingsMenu>();
        services.AddTransient<SecurityMenu>();
    }
}