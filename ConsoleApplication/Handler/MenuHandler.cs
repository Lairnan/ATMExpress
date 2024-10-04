using ConsoleApplication.Menus;

namespace ConsoleApplication.Handler;

public partial class MenuHandler
{
    public async Task<IMenu?> Switch(IMenu menu, int key)
    {
        Console.Clear();
        
        return menu switch
        {
            StartMenu => await SwitchStartMenu(key),
            MainMenu => await SwitchMainMenu(key),
            SettingsMenu => await SwitchSettingsMenu(key),
            SecurityMenu => await SwitchSecurityMenu(key),
            WithdrawMenu => await SwitchWithdrawMenu(key),
            DepositMenu => await SwitchDepositMenu(key),
            _ => null
        };

    }

    #region SwitchMenus.
    private Task<IMenu> SwitchSettingsMenu(int key)
    {
        return Task.FromResult<IMenu>(key switch
        {
            (int)SettingsMenuInfo.Back => IoC.Resolve<MainMenu>(),
            (int)SettingsMenuInfo.AccountInformation => IoC.Resolve<SettingsMenu>(),
            (int)SettingsMenuInfo.SecuritySettings => IoC.Resolve<MainMenu>(),
            (int)SettingsMenuInfo.ChangeLanguage => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SettingsMenu>(),
        });
    }

    private Task<IMenu> SwitchSecurityMenu(int key)
    {
        return Task.FromResult<IMenu>(key switch
        {
            (int)SecurityMenuInfo.Back => IoC.Resolve<SecurityMenu>(),
            (int)SecurityMenuInfo.ChangePassword => IoC.Resolve<SettingsMenu>(),
            (int)SecurityMenuInfo.ChangeName => IoC.Resolve<MainMenu>(),
            _ => IoC.Resolve<SecurityMenu>(),
        });
    }
    #endregion
}