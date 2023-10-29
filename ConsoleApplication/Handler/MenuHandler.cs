using ConsoleApplication.Menus;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    public async Task<IMenu?> Switch(IMenu menu, byte key)
    {
        Console.Clear();
        
        return menu switch
        {
            StartMenu => await SwitchStartMenu(key),
            MainMenu => await SwitchMainMenu(key),
            SettingsMenu => await SwitchSettingsMenu(key),
            SecurityMenu => await SwitchSecurityMenu(key),
            _ => null
        };

    }

    #region SwitchMenus.
    private async Task<IMenu?> SwitchStartMenu(byte key)
    {
        return key switch
        {
            (byte)StartMenuInfo.Exit => null,
            (byte)StartMenuInfo.Authorize => await Authorization(),
            (byte)StartMenuInfo.Register => await Registration(),
            _ => IoC.Resolve<StartMenu>(),
        };
    }

    private Task<IMenu?> SwitchMainMenu(byte key)
    {
        return Task.FromResult<IMenu?>(key switch
        {
            (byte)MainMenuInfo.Exit => null,
            (byte)MainMenuInfo.Settings => IoC.Resolve<SettingsMenu>(),
            (byte)MainMenuInfo.Products => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.ViewBalance => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.WithdrawCash => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.DepositCash => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.QuickTransfer => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.TransactionHistory => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.CardlessCash => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.ManageCards => IoC.Resolve<MainMenu>(),
            (byte)MainMenuInfo.PromotionsAndOffers => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<MainMenu>(),
        });
    }

    private Task<IMenu> SwitchSettingsMenu(byte key)
    {
        return Task.FromResult<IMenu>(key switch
        {
            (byte)SettingsMenuInfo.Back => IoC.Resolve<MainMenu>(),
            (byte)SettingsMenuInfo.AccountInformation => IoC.Resolve<SettingsMenu>(),
            (byte)SettingsMenuInfo.SecuritySettings => IoC.Resolve<MainMenu>(),
            (byte)SettingsMenuInfo.ChangeLanguage => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SettingsMenu>(),
        });
    }

    private Task<IMenu> SwitchSecurityMenu(byte key)
    {
        return Task.FromResult<IMenu>(key switch
        {
            (byte)SecurityMenuInfo.Back => IoC.Resolve<SecurityMenu>(),
            (byte)SecurityMenuInfo.ChangePassword => IoC.Resolve<SettingsMenu>(),
            (byte)SecurityMenuInfo.ChangeName => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SecurityMenu>(),
        });
    }
    #endregion
}