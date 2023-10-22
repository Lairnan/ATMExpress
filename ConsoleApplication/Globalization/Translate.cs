using System.Xml.Linq;

namespace ConsoleApplication.Globalization;

public static class Translate
{
    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new();
    private static Languages CultureLanguage { get; set; } = Languages.En;
    
    static Translate()
    {
        var doc = XDocument.Load("Globalization/languages.xml");

        foreach (var el in doc.Root!.Elements("tr"))
        {
            var id = el.Attribute("id")!.Value;
            var translations = el.Elements()
                .ToDictionary(s => s.Name.LocalName,
                    s => s.Value);

            Translations.TryAdd(id, translations);
        }
    }

    public static string GetString(string key)
    {
        if (!Translations.TryGetValue(key, out var translations)
            || !translations.TryGetValue(CultureLanguage.ToString().ToLower(), out var translation))
            return key;

        return translation;
    }

    public static void SetLanguage(Languages lang)
    {
        CultureLanguage = lang;
    }
}

public enum Languages
{
    Ru,
    En,
}