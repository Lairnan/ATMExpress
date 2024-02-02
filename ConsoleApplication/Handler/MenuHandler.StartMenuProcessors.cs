using ConsoleApplication.Menus;
using CSA.Behaviors;
using CSA.DTO.Handlers;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using CSA.Entities;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private async Task<IMenu?> SwitchStartMenu(int key)
    {
        return key switch
        {
            (int)StartMenuInfo.Exit => null,
            (int)StartMenuInfo.Authorize => await Authorization(),
            (int)StartMenuInfo.Register => await Registration(),
            _ => IoC.Resolve<StartMenu>(),
        };
    }
    
    private async Task<IMenu> Authorization()
    {
        var user = GetUserInfo();
        var request = new LoginRequest(user.Login, user.Password);
        var response = await RequestHandler.Login(request);
        
        if (response is ApiResponse apiResponse)
        {
            Console.WriteLine(apiResponse.Message);
            Application.WaitEnter();
            return IoC.Resolve<StartMenu>();
        }

        var loginResponse = response as LoginResponse;
        Application.User = loginResponse;
        return IoC.Resolve<MainMenu>();
    }
    
    private async Task<IMenu> Registration()
    {
        var user = GetUserInfo();
        var request = new RegisterRequest(user.Login, user.Password);
        var response = await RequestHandler.Register(request);
        
        if (!response.Success)
        {
            Console.WriteLine(response.Message);
            Application.WaitEnter();
        }
        
        return IoC.Resolve<StartMenu>();
    }

    private static User GetUserInfo()
    {
        var login = EnterData("input_login");
        if (string.IsNullOrWhiteSpace(login)) return null!;
        var password = EnterData("input_password");
        if (string.IsNullOrWhiteSpace(password)) return null!;
        
        return new User
        {
            Login = login,
            Password = password,
            AdminLevel = Level.User
        };
    }

    private static string EnterData(string message)
    {
        Console.Write(Translate.GetString(message));
        var data = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(data)) return data;
        
        Console.Write(Translate.GetString("try_login_again"));
        do
        {
            var keyInfo = Console.ReadKey().Key;
            Console.WriteLine();
            if (keyInfo == ConsoleKey.D1) return EnterData(message);
            if (keyInfo == ConsoleKey.D2) return "";
            
            Console.WriteLine(Translate.GetString("input_error"));
        } while (true);
    }
}