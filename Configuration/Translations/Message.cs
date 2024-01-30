using System.Xml.Linq;

namespace Configuration.Translations;

public class Message(string filePath) : AMessage(filePath);

public abstract class AMessage
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations = new();
    public readonly string FilePath;
    
    protected AMessage(string filePath)
    {
        if (!Path.IsPathRooted(filePath))
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
        FilePath = filePath;
        var doc = XDocument.Load(filePath);

        foreach (var el in doc.Elements("languages").Elements("tr"))
        {
            var id = el.Attribute("id")!.Value;
            var languages = el.Elements()
                .ToDictionary(s => s.Name.LocalName,
                    s => s.Value);

            _translations.TryAdd(id, languages);
        }
    }
    
    public bool Contain(string key)
        => _translations.ContainsKey(key);
    
    public string GetString(string key, Languages language)
    {
        if (!_translations.TryGetValue(key, out var languages)
            || !languages.TryGetValue(language.ToString().ToLower(), out var translation))
            return key;

        return translation;
    }
}