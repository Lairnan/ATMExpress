using Configuration;
using CSA;

namespace AccessHub.Models;

public class UserToken : ObservableProperty
{
    private bool _valid;
    private string? _token;
    private Guid? _userId;
    [NonSerialized]
    private Languages _language;

    public UserToken()
    {
    }

    public UserToken(string? token)
    {
        _token = token;
    }

    public UserToken(string? token, Guid? userId)
    {
        _token = token;
        _userId = userId;
    }

    public UserToken(bool valid, string? token, Guid? userId)
    {
        _valid = valid;
        _token = token;
        _userId = userId;
    }

    public bool Valid
    {
        get => _valid;
        set => SetProperty(ref _valid, value);
    }

    public string? Token
    {
        get => _token;
        set => SetProperty(ref _token, value);
    }

    public Guid? UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    public Languages Language
    {
        get => _language;
        set => SetProperty(ref _language, value);
    }
}