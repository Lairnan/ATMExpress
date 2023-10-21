using ConsoleApplication.Globalization;
using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication;

public class Application
{
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
    
    public IMenu? Menu(IMenu menu)
    {
        string keyChar;
        int keyInt;
        
        do
        {
            PrintMenu(menu);
            Console.Write($"{Translate.GetString("InsertKey")}: ");
            keyChar = Console.ReadKey().KeyChar.ToString();
        } while (!int.TryParse(keyChar, out keyInt));

        return _provider.GetRequiredService<MenuHandler>().Switch(menu, keyInt);
    }
}