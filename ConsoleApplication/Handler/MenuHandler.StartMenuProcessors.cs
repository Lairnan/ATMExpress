using ConsoleApplication.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private IMenu? Authorization()
    {
        return IoC.Resolve<MainMenu>();
    }
    
    private IMenu? Registration()
    {
        return IoC.Resolve<MainMenu>();
    }
}