using Configuration;
using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using CSA.DTO.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

public class Application
{
    public static LoginResponse? User { get; set; }
    
    private readonly IServiceProvider _provider;

    public Application(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    private static void PrintMenu(IMenu menu)
    {
        for (var i = 0; i < menu.GetSize(); i++)
            Console.WriteLine($"{i}. {Translate.GetString(menu.GetName(i))}");
    }
    
    public async Task<IMenu?> Menu(IMenu menu)
    {
        Console.Clear();
        string keyChar;
        int keyInt;
        
        do
        {
            PrintMenu(menu);
            Console.Write(Translate.GetString("insert_key"));
            keyChar = Console.ReadLine() ?? "";
        } while (!int.TryParse(keyChar, out keyInt));

        return await _provider.GetRequiredService<MenuHandler>().Switch(menu, keyInt);
    }

    public static void WaitEnter()
    {
        Console.Write(Translate.GetString("press_any_key"));
        Console.ReadKey();
    }
}