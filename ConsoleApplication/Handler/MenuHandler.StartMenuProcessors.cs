using ConsoleApplication.Globalization;
using ConsoleApplication.Menus;
using CSA.DTO;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using CSA.Entities;
using Newtonsoft.Json;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private async Task<IMenu> Authorization()
    {
        var user = GetUserLogon();
        var request = new LoginRequest(user.Login, user.Password);
        var response = await RequestHandler.DoRequest(RequestType.Post, request, "auth/login");

        IMenu menu;
        if (response is not { Success: true })
        {
            if(response == null || (string.IsNullOrWhiteSpace(response.Message) && string.IsNullOrWhiteSpace(response.Data)))
                Console.WriteLine(Translate.GetString("bad_request"));
            else Console.WriteLine(Translate.GetString(response.Message));
            menu = IoC.Resolve<StartMenu>();
            Application.WaitEnter();
        }
        else
        {
            Console.WriteLine(response.Message);
            var loginResp = JsonConvert.DeserializeObject<LoginResponse>(response.Data);
            Application.User = loginResp;
            menu = IoC.Resolve<MainMenu>();
        }

        return menu;
    }
    
    private async Task<MainMenu> Registration()
    {
        return IoC.Resolve<MainMenu>();
    }

    private static User GetUserLogon()
    {
        string login;
        string password;
        while (true)
        {
            Console.Write($"{Translate.GetString("input_login")}: ");
            login = Console.ReadLine() ?? "";
            Console.Write($"{Translate.GetString("input_password")}: ");
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
            Console.WriteLine($"{Translate.GetString("input_login")}");
            login = Console.ReadLine() ?? "";
            Console.WriteLine($"{Translate.GetString("input_password")}");
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