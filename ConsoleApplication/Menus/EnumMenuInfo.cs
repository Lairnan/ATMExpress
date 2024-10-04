using System.ComponentModel;

namespace ConsoleApplication.Menus;

public enum MainMenuInfo
{
    [Description("exit")]
    Exit,
    [Description("settings")]
    Settings,
    [Description("products")]
    Products,
    [Description("view_balance")]
    ViewBalance,
    [Description("withdraw_cash")]
    WithdrawCash,
    [Description("deposit_cash")]
    DepositCash,
    [Description("quick_transfer")]
    QuickTransfer,
    [Description("transaction_history")]
    TransactionHistory,
    [Description("cardless_cash")]
    CardlessCash,
    [Description("manage_cards")]
    ManageCards,
    [Description("promotions_and_offers")]
    PromotionsAndOffers,
}

public enum StartMenuInfo
{
    [Description("exit")]
    Exit,
    [Description("authorize")]
    Authorize,
    [Description("register")]
    Register,
}

public enum SettingsMenuInfo
{
    [Description("back")]
    Back,
    [Description("account_information")]
    AccountInformation,
    [Description("security_settings")]
    SecuritySettings,
    [Description("change_language")]
    ChangeLanguage,
}

public enum SecurityMenuInfo
{
    [Description("back")]
    Back,
    [Description("change_password")]
    ChangePassword,
    [Description("change_name")]
    ChangeName,
}

public enum WithdrawMenuInfo
{
    [Description("back")]
    Back,
    [Description("select_card")]
    SelectCard
}

public enum DepositMenuInfo
{
    [Description("back")]
    Back,
    [Description("select_card")]
    SelectCard,
    [Description("create_card")]
    CreateCard
}