using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CSA.Entities;

[Table("Transactions")]
public class Transaction : Entity
{
    private string? _value;
    private TransactionType _transactionType;
    private DateTime _dateCreated;
    private Guid _cardId;
    private Card _card;
    [JsonIgnore]
    private ICollection<ProductTransaction> _productTransactions = new List<ProductTransaction>();

    public string? Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public TransactionType TransactionType
    {
        get => _transactionType;
        set => SetProperty(ref _transactionType, value);
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        set => SetProperty(ref _dateCreated, value);
    }

    public Guid CardId
    {
        get => _cardId;
        set => SetProperty(ref _cardId, value);
    }

    public Card Card
    {
        get => _card;
        set => SetProperty(ref _card, value);
    }

    [JsonIgnore]
    public virtual ICollection<ProductTransaction> ProductTransactions
    {
        get => _productTransactions;
        set => SetProperty(ref _productTransactions, value);
    }
}

public enum TransactionType
{
    Deposit,
    Withdraw,
    Sale,
}