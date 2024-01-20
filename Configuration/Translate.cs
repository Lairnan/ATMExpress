using System.Reflection;
using System.Xml.Linq;

namespace Configuration;

public static class Translate
{
    private static readonly Dictionary<string, Dictionary<string, string>> translations = new();
    public static Languages CultureLanguage { get; private set; } = Languages.En;

    static Translate()
    {
        var doc = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/languages.xml"));

        foreach (var el in doc.Elements("languages").Elements("tr"))
        {
            var id = el.Attribute("id")!.Value;
            var languages = el.Elements()
                .ToDictionary(s => s.Name.LocalName,
                    s => s.Value);

            translations.TryAdd(id, languages);
        }
    }

    public static string GetString(string key)
    {
        if (!translations.TryGetValue(key, out var languages)
            || !languages.TryGetValue(CultureLanguage.ToString().ToLower(), out var translation))
            return key;

        return translation;
    }

    public static string GetString(string key, params object[] args)
    {
        if (!translations.TryGetValue(key, out var languages)
            || !languages.TryGetValue(CultureLanguage.ToString().ToLower(), out var translation))
            return key;
        
        return string.Format(translation, args);
    }

    public static void SetLanguage(Languages lang)
    {
        CultureLanguage = lang;
    }
}