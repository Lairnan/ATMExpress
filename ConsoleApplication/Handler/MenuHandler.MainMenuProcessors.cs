using ConsoleApplication.Globalization;
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
        var response = await RequestHandler.DoRequest(RequestType.Get, "products/getall", Application.User);
        if (!response.Success)
        {
            Console.WriteLine(Translate.GetString(response.Message));
        }
        else
        {
            var products = JsonConvert.DeserializeObject<IList<Product>>(response.Data);

            if (products == null || products.Count == 0)
            {
                Console.WriteLine(Translate.GetString("no_products"));
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine(product.ToString());
                }
            }
        }

        Application.WaitEnter();
        return IoC.Resolve<MainMenu>();
    }
}