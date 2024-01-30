using System.ComponentModel.DataAnnotations.Schema;
using Configuration;
using Newtonsoft.Json;

namespace CSA.Entities;

[Table("Products")]
public class Product : Entity
{
    private string _name;
    private string _description;
    private decimal _price;
    private decimal _weight;
    private DateTime _dateCreated;
    [JsonIgnore]
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
    
    [JsonIgnore]
    public virtual ICollection<ProductTransaction> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }
    
    public override string ToString()
        => Translate.GetString("product_info", this.Id, this.Name, this.Description, this.Price.ToString("0.00"), this.Weight.ToString("0.00"), this.DateCreated.ToString("dd.MM.yyyy"));
}
