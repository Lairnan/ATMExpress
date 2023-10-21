namespace ConsoleApplication.Menus;

public class MainMenu : IMenu
{
    public string GetName(int value) => ((MainMenuInfo)value).ToString();
    public int GetSize() => Enum.GetNames(typeof(MainMenuInfo)).Length;
}

public class StartMenu : IMenu
{
    public string GetName(int value) => ((StartMenuInfo)value).ToString();
    public int GetSize() => Enum.GetNames(typeof(StartMenuInfo)).Length;
}

public class SettingsMenu : IMenu
{
    public string GetName(int value) => ((SettingsMenuInfo)value).ToString();
    public int GetSize() => Enum.GetNames(typeof(SettingsMenuInfo)).Length;
}

public class SecurityMenu : IMenu
{
    public string GetName(int value) => ((SecurityMenuInfo)value).ToString();
    public int GetSize() => Enum.GetNames(typeof(SecurityMenuInfo)).Length;
}