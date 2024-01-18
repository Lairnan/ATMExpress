using ConsoleApplication;
using ConsoleApplication.Globalization;
using ConsoleApplication.Handler;
using ConsoleApplication.Menus;
using CSA.DTO.Handlers;

args.HandleArguments();
        
IMenu? result = new StartMenu();
var app = IoC.Resolve<Application>();
do
{
    result = await app.Menu(result);
    Thread.Sleep(250);
    Console.Clear();
} while (result != null);

if (Application.User != null)
{
    Console.WriteLine(Translate.GetString("logout"));
    var response = await RequestHandler.Logout(Application.User);
    Console.WriteLine(Translate.GetString(response.Message));
}
Console.Write($"\n{Translate.GetString("close_app")}");
await Task.Delay(1000);