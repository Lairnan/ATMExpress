using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseManagement.Interfaces;

namespace DatabaseManagement.Entities;

[Table("Users")]
public class User : Entity
{
    private string _login;
    private string _password;
    private DateTime _dateCreated = DateTime.Now;
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

    public virtual ICollection<Card> Cards
    {
        get => _cards;
        set => SetProperty(ref _cards, value);
    }
}