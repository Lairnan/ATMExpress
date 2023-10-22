using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseManagement.Interfaces;

namespace DatabaseManagement.Entities;

[Table("Cards")]
public class Card : Entity
{
    private string _cardNumber;
    private decimal _balance;
    private Guid? _cardless;
    private Guid _userId;
    private User? _user;
    private ICollection<Transaction> _transactions = new ObservableCollection<Transaction>();

    public string CardNumber
    {
        get => _cardNumber;
        set => SetProperty(ref _cardNumber, value);
    }

    public decimal Balance
    {
        get => _balance;
        set => SetProperty(ref _balance, value);
    }

    public Guid? Cardless
    {
        get => _cardless;
        set => SetProperty(ref _cardless, value);
    }

    public Guid UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    public virtual User? User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public virtual ICollection<Transaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }
}