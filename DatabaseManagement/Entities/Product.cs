namespace DatabaseManagement.Entities;

public class Product
{
    private int _id;
    private string _name;
    private string _description;
    private decimal _price;
    private int _quantity;
    private DateTime _dateCreated;

    public Product()
    {
        _id = 0;
        _name = string.Empty;
    }

    public int Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public decimal Price { get { return _price; } set { _price = value; } }
    public int Quantity { get { return _quantity;} set { _quantity = value; } }
    public DateTime DateCreated { get { return _dateCreated; } set { _dateCreated = value; } }
}
