using System.ComponentModel.DataAnnotations.Schema;

namespace CSA.Entities;

[Table("Products")]
public class Product : Entity
{
    private string _name;
    private string _description;
    private decimal _price;
    private decimal _weight;
    private DateTime _dateCreated;
    private ICollection<ProductTransaction> _transactions;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public decimal Weight
    {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        set => SetProperty(ref _dateCreated, value);
    }
    
    public virtual ICollection<ProductTransaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }
}
