using ConsoleApplication.Menus;
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
        var user = GetUserLogon();
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
        var user = GetUserRegister();
        var request = new RegisterRequest(user.Login, user.Password);
        var response = await RequestHandler.Register(request);
        
        if (!response.Success)
        {
            Console.WriteLine(response.Message);
            Application.WaitEnter();
        }
        
        return IoC.Resolve<StartMenu>();
    }

    private static User GetUserLogon()
    {
        string login;
        string password;
        while (true)
        {
            Console.Write(Translate.GetString("input_login"));
            login = Console.ReadLine() ?? "";
            Console.Write(Translate.GetString("input_password"));
            password = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) break;
        }

        return new User
        {
            Login = login,
            Password = password
        };
    }

    private static User GetUserRegister()
    {
        string login;
        string password;
        while (true)
        {
            Console.Write($"{Translate.GetString("input_login")}");
            login = Console.ReadLine() ?? "";
            Console.Write($"{Translate.GetString("input_password")}");
            password = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)) break;
        }

        return new User
        {
            Login = login,
            Password = password
        };
    }
}