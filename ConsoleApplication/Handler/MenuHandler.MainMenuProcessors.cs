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
            (int)MainMenuInfo.ViewBalance => await ViewBalance(),
            (int)MainMenuInfo.WithdrawCash => await WithdrawDepositCash(false),
            (int)MainMenuInfo.DepositCash => await WithdrawDepositCash(true),
            (int)MainMenuInfo.QuickTransfer => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.TransactionHistory => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.CardlessCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.ManageCards => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.PromotionsAndOffers => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        });
    }

    private async Task<IMenu> ViewBalance()
    {
        Console.Clear();
        if (Application.User == null)
        {
            return Extension.ThrowError<StartMenu>("not_authorized");
        }
        
        var response = await RequestHandler.DoRequest(RequestType.Get, $"cards/get-balance/{Application.User.UserId}", Application.User);
        if (!response.Success)
        {
            return Extension.ThrowError<MainMenu>(response.Message);
        }

        if (!decimal.TryParse(response.Data, out var balance))
        {
            return Extension.ThrowError<MainMenu>("fail_decimal_convert");
        }
        
        Console.WriteLine(Translate.GetString("balance_info", balance));
        Application.WaitEnter();
        return IoC.Resolve<MainMenu>();
    }

    private async Task<IMenu> ViewProducts()
    {
        Console.Clear();
        if (Application.User == null)
        {
            return Extension.ThrowError<StartMenu>("not_authorized");
        }

        var response = await RequestHandler.DoRequest(RequestType.Get, "products/count", Application.User);
        if (!response.Success)
        {
            return Extension.ThrowError<MainMenu>(response.Message);
        }

        var count = JsonConvert.DeserializeObject<int>(response.Data)!;
        if (count < 1)
        {
            return Extension.ThrowError<MainMenu>("no_products");
        }

        return await Extension.DisplayContentByPages<MainMenu, Product>(count, "products/get-all");
    }

    private async Task<IMenu> WithdrawDepositCash(bool isDeposit)
    {
        Console.Clear();
        if (Application.User == null)
        {
            return Extension.ThrowError<StartMenu>("not_authorized");
        }
        
        var response = await RequestHandler.DoRequest(RequestType.Get, $"cards/count?userId={Application.User.UserId}", Application.User);
        if (!response.Success)
        {
            return Extension.ThrowError<MainMenu>(response.Message);
        }

        return isDeposit ? IoC.Resolve<DepositMenu>() : IoC.Resolve<WithdrawMenu>();
    }
}