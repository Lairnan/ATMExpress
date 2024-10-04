using ConsoleApplication.Behaviors;
using ConsoleApplication.Menus;
using CSA.DTO;
using CSA.Entities;
using Newtonsoft.Json;
using RequestHandler = CSA.DTO.Handlers.RequestHandler;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    public async Task<IMenu?> SwitchWithdrawMenu(int key)
    {
        return key switch
        {
            (int)WithdrawMenuInfo.Back => null,
            (int)WithdrawMenuInfo.SelectCard => await SelectCard<WithdrawMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        };
    }
    
    public async Task<IMenu?> SwitchDepositMenu(int key)
    {
        return key switch
        {
            (int)DepositMenuInfo.Back => null,
            (int)DepositMenuInfo.SelectCard => await SelectCard<DepositMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        };
    }

    private async Task<IMenu?> SelectCard<TDrop>()
        where TDrop : IMenu
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
        
        var count = JsonConvert.DeserializeObject<int>(response.Data)!;
        if (count < 1)
        {
            return Extension.ThrowError<MainMenu>("no_cards");
        }

        while (true)
        {
            var choose = Extension.TryGetInt(Translate.GetString("choose_input_or_view"));
            switch (choose)
            {
                case 1:
                    await Extension.DisplayContentByPages<MainMenu, Card>(count,
                        $"cards/get-all?userId={Application.User.UserId}", Application.User.UserId.ToString());
                    break;
                case 2:
                    var cardNumber = Extension.TryGetInput(Translate.GetString("enter_card_number"));
                    if (string.IsNullOrWhiteSpace(cardNumber))
                    {
                        return Extension.ThrowError<TDrop>("card_number_empty");
                    }
                    return await VerifyCard<TDrop>(cardNumber);
                case 0:
                    return IoC.Resolve<MainMenu>();
                default:
                    Extension.ThrowError<TDrop>("invalid_input");
                    break;
            }
        }
    }

    private async Task<IMenu?> VerifyCard<TDrop>(string input)
        where TDrop : IMenu
    {
        
        var response = await RequestHandler.DoRequest(RequestType.Get, $"cards/{input}", Application.User!);
        if (!response.Success)
        {
            return Extension.ThrowError<TDrop>(response.Message);
        }

        var card = JsonConvert.DeserializeObject<Card>(response.Data);
        if (card == null)
        {
            return Extension.ThrowError<TDrop>("card_not_found");
        }

        return await (typeof(TDrop).FullName == typeof(WithdrawMenu).FullName ? WithdrawCash(card) : DepositCash(card));
    }

    private async Task<IMenu?> WithdrawCash(Card card)
    {
        return IoC.Resolve<WithdrawMenu>();
    }

    private async Task<IMenu?> DepositCash(Card card)
    {
        return IoC.Resolve<WithdrawMenu>();
    }
}