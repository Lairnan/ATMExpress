namespace ConsoleApplication.Menus;

public enum MainMenuInfo
{
    Exit,
    Settings,
    Products,
    ViewBalance,
    WithdrawCash,
    DepositCash,
    QuickTransfer,
    TransactionHistory,
    CardlessCash,
    ManageCards,
    PromotionsAndOffers,
}

public enum StartMenuInfo
{
    Exit,
    Authorize,
    Register,
}

public enum SettingsMenuInfo
{
    Back = 1,
    AccountInformation,
    SecuritySettings,
    ChangeLanguage,
}

public enum SecurityMenuInfo
{
    Back = 1,
    ChangePassword,
    ChangeName,
}