using ConsoleApplication.Handler;

namespace ConsoleApplication.Menus;

public class MainMenu : IMenu
{
    public string GetName(int value) => ((MainMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(MainMenuInfo)).Length;
}

public class StartMenu : IMenu
{
    public string GetName(int value) => ((StartMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(StartMenuInfo)).Length;
}

public class SettingsMenu : IMenu
{
    public string GetName(int value) => ((SettingsMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(SettingsMenuInfo)).Length;
}

public class SecurityMenu : IMenu
{
    public string GetName(int value) => ((SecurityMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(SecurityMenuInfo)).Length;
}

public class WithdrawMenu : IMenu
{
    public string GetName(int value) => ((WithdrawMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(WithdrawMenuInfo)).Length;
}

public class DepositMenu : IMenu
{
    public string GetName(int value) => ((DepositMenuInfo)value).GetDescription();
    public int GetSize() => Enum.GetNames(typeof(DepositMenuInfo)).Length;
}