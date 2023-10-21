using ConsoleApplication.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    private readonly IServiceProvider _provider;

    public MenuHandler(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IMenu? Switch(IMenu menu, int key)
    {
        return menu switch
        {
            StartMenu => SwitchStartMenu(key),
            MainMenu => SwitchMainMenu(key),
            _ => SwitchStartMenu(key)
        };

    }

    #region SwitchMenus.
    private IMenu? SwitchStartMenu(int key)
    {
        return key switch
        {
            (int)StartMenuInfo.Exit => null,
            (int)StartMenuInfo.Authorize => Authorization(),
            (int)StartMenuInfo.Register => Registration(),
            _ => _provider.GetRequiredService<StartMenu>(),
        };
    }

    private IMenu? SwitchMainMenu(int key)
    {
        return key switch
        {
            (int)MainMenuInfo.Exit => null,
            (int)MainMenuInfo.Settings => _provider.GetRequiredService<SettingsMenu>(),
            (int)MainMenuInfo.Products => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.ViewBalance => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.WithdrawCash => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.DepositCash => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.QuickTransfer => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.TransactionHistory => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.CardlessCash => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.ManageCards => _provider.GetRequiredService<MainMenu>(),
            (int)MainMenuInfo.PromotionsAndOffers => _provider.GetRequiredService<MainMenu>(),
            _ => _provider.GetRequiredService<StartMenu>(),
        };
    }

    private IMenu? SwitchSettingsMenu(int key)
    {
        return key switch
        {
            (int)SettingsMenuInfo.Back => _provider.GetRequiredService<MainMenu>(),
            (int)SettingsMenuInfo.AccountInformation => _provider.GetRequiredService<SettingsMenu>(),
            (int)SettingsMenuInfo.SecuritySettings => _provider.GetRequiredService<MainMenu>(),
            (int)SettingsMenuInfo.ChangeLanguage => _provider.GetRequiredService<MainMenu>(),
            _ => _provider.GetRequiredService<StartMenu>(),
        };
    }

    private IMenu? SwitchSecurityMenu(int key)
    {
        return key switch
        {
            (int)SecurityMenuInfo.Back => _provider.GetRequiredService<SecurityMenu>(),
            (int)SecurityMenuInfo.ChangePassword => _provider.GetRequiredService<SettingsMenu>(),
            (int)SecurityMenuInfo.ChangeName => _provider.GetRequiredService<MainMenu>(),
            _ => _provider.GetRequiredService<StartMenu>(),
        };
    }
    #endregion
}