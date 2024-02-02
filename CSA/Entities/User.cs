using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using CSA.Behaviors;
using Newtonsoft.Json;

namespace CSA.Entities;

[Table("Users")]
public class User : Entity
{
    private string _login;
    private string _password;
    private DateTime _dateCreated = DateTime.Now;
    private Level _adminLevel = 0;
    [JsonIgnore]
    private ICollection<Card> _cards = new ObservableCollection<Card>();

    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        set => SetProperty(ref _dateCreated, value);
    }

    public Level AdminLevel
    {
        get => _adminLevel;
        set => SetProperty(ref _adminLevel, value);
    }

    [JsonIgnore]
    public virtual ICollection<Card> Cards
    {
        get => _cards;
        set => SetProperty(ref _cards, value);
    }
}