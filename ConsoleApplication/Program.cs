using ConsoleApplication;
using ConsoleApplication.Globalization;
using ConsoleApplication.Handler;
using ConsoleApplication.Menus;

args.HandleArguments();
        
IMenu? result = new StartMenu();
var app = IoC.Resolve<Application>();
do
{
    result = app.Menu(result);
    Thread.Sleep(250);
    Console.Clear();
} while (result != null);

Console.Write($"\n{Translate.GetString("CloseApp")}");
await Task.Delay(1000);