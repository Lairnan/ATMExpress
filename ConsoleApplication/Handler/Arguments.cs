using ConsoleApplication.Globalization;

namespace ConsoleApplication.Handler;

public static class Arguments
{
    public static void HandleArguments(this IEnumerable<string> args)
    {
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "RU":
                    Translate.SetLanguage(Languages.Ru);
                    break;
                case "EN":
                    Translate.SetLanguage(Languages.En);
                    break;
            }
        }
    }
}