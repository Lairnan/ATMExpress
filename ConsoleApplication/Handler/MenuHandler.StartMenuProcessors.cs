using ConsoleApplication.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private IMenu? Authorization()
    {
        return _provider.GetRequiredService<MainMenu>();
    }
    
    private IMenu? Registration()
    {
        return _provider.GetRequiredService<MainMenu>();
    }
}