using ConsoleApplication.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    public IMenu? Switch(IMenu menu, int key)
    {
        return menu switch
        {
            StartMenu => SwitchStartMenu(key),
            MainMenu => SwitchMainMenu(key),
            SettingsMenu => SwitchSettingsMenu(key),
            SecurityMenu => SwitchSecurityMenu(key),
            _ => null
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
            _ => IoC.Resolve<StartMenu>(),
        };
    }

    private IMenu? SwitchMainMenu(int key)
    {
        return key switch
        {
            (int)MainMenuInfo.Exit => null,
            (int)MainMenuInfo.Settings => IoC.Resolve<SettingsMenu>(),
            (int)MainMenuInfo.Products => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.ViewBalance => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.WithdrawCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.DepositCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.QuickTransfer => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.TransactionHistory => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.CardlessCash => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.ManageCards => IoC.Resolve<MainMenu>(),
            (int)MainMenuInfo.PromotionsAndOffers => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        };
    }

    private IMenu SwitchSettingsMenu(int key)
    {
        return key switch
        {
            (int)SettingsMenuInfo.Back => IoC.Resolve<MainMenu>(),
            (int)SettingsMenuInfo.AccountInformation => IoC.Resolve<SettingsMenu>(),
            (int)SettingsMenuInfo.SecuritySettings => IoC.Resolve<MainMenu>(),
            (int)SettingsMenuInfo.ChangeLanguage => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SettingsMenu>(),
        };
    }

    private IMenu SwitchSecurityMenu(int key)
    {
        return key switch
        {
            (int)SecurityMenuInfo.Back => IoC.Resolve<SecurityMenu>(),
            (int)SecurityMenuInfo.ChangePassword => IoC.Resolve<SettingsMenu>(),
            (int)SecurityMenuInfo.ChangeName => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SecurityMenu>(),
        };
    }
    #endregion
}