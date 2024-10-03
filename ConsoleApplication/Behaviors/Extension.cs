using ConsoleApplication.Menus;
using CSA;
using CSA.DTO;
using CSA.Entities;
using Newtonsoft.Json;
using RequestHandler = CSA.DTO.Handlers.RequestHandler;

namespace ConsoleApplication.Behaviors;

public static class Extension
{
    public static int TryGetInt(string enterText)
    {
        do
        {
            Console.Write(enterText);
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result)) return result;
            Console.WriteLine(Translate.GetString("input_error"));
        } while (true);
    }
    
    public static IMenu ThrowError<T>(string message)
        where T : IMenu
    {
        Console.WriteLine(Translate.GetString(message));
        Application.WaitEnter();
        return IoC.Resolve<T>();
    }

    public static async Task<bool> DisplayContent<T>(int currentPage, int pageSize, string requestPath, params string[]? parameters)
        where T : Entity
    {
        var res = true;
        Console.WriteLine(new string('-', 50));
        var response = await RequestHandler.DoRequest(RequestType.Get, $"{requestPath}?page={currentPage}&pageSize={pageSize}", Application.User!, parameters);
        if (!response.Success)
        {
            Console.WriteLine(response.Message);
            Application.WaitEnter();
            res = false;
        }

        if (res)
        {
            var products = JsonConvert.DeserializeObject<IList<T>>(response.Data)!;
            foreach (var product in products)
                Console.WriteLine(product.ToString());
        }

        Console.WriteLine(new string('-', 50));
        return true;
    }

    public static async Task<IMenu> DisplayContentByPages<TMenu, TContent>(int count, string requestPath, params string[]? parameters)
        where TMenu : IMenu
        where TContent : Entity
    {
        const int pageSize = 20;
        var pageCount = count < pageSize ? 1 : count / pageSize + (count % pageSize > 0 ? 1 : 0);
        var currentPage = 1;
        
        Console.WriteLine(Translate.GetString("total_items", count));
        Console.WriteLine(Translate.GetString("total_pages", pageCount));
        Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
        if (pageCount < 2)
        {
            if(await DisplayContent<TContent>(currentPage, pageSize, requestPath, parameters))
                Application.WaitEnter();
            return IoC.Resolve<TMenu>();
        }
        
        do
        {
            if (!await DisplayContent<TContent>(currentPage, pageSize, requestPath, parameters))
                return IoC.Resolve<TMenu>();

            var inputPage = TryGetInt(Translate.GetString("enter_page_number"));
            while (inputPage < 1 || inputPage > pageCount)
            {
                if (inputPage == 0)
                {
                    Application.WaitEnter();
                    return IoC.Resolve<TMenu>();
                }
                
                Console.Clear();
                Console.WriteLine(Translate.GetString("beyond_pages", pageCount));
                Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
                inputPage = TryGetInt(Translate.GetString("enter_page_number"));
            }

            currentPage = inputPage;
            Console.Clear();
            Console.WriteLine(Translate.GetString("total_items", count));
            Console.WriteLine(Translate.GetString("total_pages", pageCount));
            Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
        } while (true);
    }
}