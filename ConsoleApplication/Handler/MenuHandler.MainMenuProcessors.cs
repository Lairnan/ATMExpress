using ConsoleApplication.Behaviors;
using ConsoleApplication.Menus;
using CSA.DTO;
using CSA.Entities;
using Newtonsoft.Json;
using RequestHandler = CSA.DTO.Handlers.RequestHandler;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private async Task<IMenu?> SwitchMainMenu(int key)
    {
        return await Task.FromResult(key switch
        {
            (int)MainMenuInfo.Exit => null,
            (int)MainMenuInfo.Settings => IoC.Resolve<SettingsMenu>(),
            (int)MainMenuInfo.Products => await ViewProducts(),
            (int)MainMenuInfo.ViewBalance => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.WithdrawCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.DepositCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.QuickTransfer => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.TransactionHistory => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.CardlessCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.ManageCards => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.PromotionsAndOffers => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        });
    }

    private async Task<IMenu> ViewProducts()
    {
        Console.Clear();
        if (Application.User == null)
        {
            Console.WriteLine(Translate.GetString("not_authorized"));
            Application.WaitEnter();
            return IoC.Resolve<StartMenu>();
        }

        var response = await RequestHandler.DoRequest(RequestType.Get, "products/count", Application.User);
        if (!response.Success)
        {
            Console.WriteLine(response.Message);
            Application.WaitEnter();
            return IoC.Resolve<MainMenu>();
        }

        var count = JsonConvert.DeserializeObject<int>(response.Data)!;
        if (count < 1)
        {
            Console.WriteLine(Translate.GetString("no_products"));
            Application.WaitEnter();
            return IoC.Resolve<MainMenu>();
        }
        
        const int pageSize = 20;
        var pageCount = count < pageSize ? 1 : count / pageSize + (count % pageSize > 0 ? 1 : 0);
        var currentPage = 1;
        
        Console.WriteLine(Translate.GetString("total_products", count));
        Console.WriteLine(Translate.GetString("total_pages", pageCount));
        Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
        if (pageCount < 2)
        {
            if(await DisplayedProducts(currentPage, pageSize))
                Application.WaitEnter();
            return IoC.Resolve<MainMenu>();
        }
        
        do
        {
            if (!await DisplayedProducts(currentPage, pageSize))
                return IoC.Resolve<MainMenu>();

            var inputPage = Extension.TryGetInt(Translate.GetString("enter_page_number"));
            while (inputPage < 1 || inputPage > pageCount)
            {
                if (inputPage == 0)
                {
                    Application.WaitEnter();
                    return IoC.Resolve<MainMenu>();
                }
                
                Console.Clear();
                Console.WriteLine(Translate.GetString("beyond_pages", pageCount));
                Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
                inputPage = Extension.TryGetInt(Translate.GetString("enter_page_number"));
            }

            currentPage = inputPage;
            Console.Clear();
            Console.WriteLine(Translate.GetString("total_products", count));
            Console.WriteLine(Translate.GetString("total_pages", pageCount));
            Console.WriteLine(Translate.GetString("page_number", currentPage, pageCount));
        } while (true);
    }

    private async Task<bool> DisplayedProducts(int currentPage, int pageSize)
    {
        Console.WriteLine(new string('-', 50));
        var response = await RequestHandler.DoRequest(RequestType.Get, $"products/getall?page={currentPage}&pageSize={pageSize}", Application.User!);
        if (!response.Success)
        {
            Console.WriteLine(response.Message);
            Application.WaitEnter();
            return false;
        }
            
        var products = JsonConvert.DeserializeObject<IList<Product>>(response.Data)!;
        foreach (var product in products)
            Console.WriteLine(product.ToString());
        
        Console.WriteLine(new string('-', 50));
        return true;
    }
}