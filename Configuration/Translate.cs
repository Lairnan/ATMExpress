using System.Collections.ObjectModel;
using Configuration.Translations;

namespace Configuration;

public static class Translate
{
    private static readonly IMessage _defaultMessage;
    private static readonly ObservableCollection<IMessage> _messages;
    public static Languages CultureLanguage { get; private set; } = Languages.En;

    static Translate()
    {
        _defaultMessage = new Message(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/languages.xml"));
        _messages = new ObservableCollection<IMessage>();
    }

    #region LoadMessage
    public static void LoadMessage(string filePath)
    {
        if(_messages.FirstOrDefault(s => s.FilePath == filePath) == null)
            _messages.Add(new Message(filePath));
    }
    
    public static void LoadMessages(IEnumerable<string> filePaths)
    {
        foreach (var filePath in filePaths)
            LoadMessage(filePath);
    }

    public static void LoadMessage(IMessage message)
    {
        if (_messages.FirstOrDefault(s => s.FilePath == message.FilePath) == null)
            _messages.Add(message);
    }
    
    public static void LoadMessages(IEnumerable<IMessage> messages)
    {
        foreach (var message in messages)
            LoadMessage(message);
    }
    #endregion

    public static string GetString(string key)
    {
        var message = GetMessage(key);
        return message == null ? key : message.GetString(key, CultureLanguage);
    }

    public static string GetString(string key, params object[] args)
    {
        var text = GetString(key);
        return text == key ? key : string.Format(text, args);
    }
    
    private static IMessage? GetMessage(string key)
        => _defaultMessage.Contain(key) ? _defaultMessage : _messages.FirstOrDefault(s => s.Contain(key));

    public static void SetLanguage(Languages lang)
        => CultureLanguage = lang;
}