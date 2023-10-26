namespace CSA.Entities;

public class ProductTransaction : Entity
{
    private Guid _productsId;
    private Guid _transactionId;
    private int? _quantity;
    private Product _products = null!;
    private Transaction _transaction = null!;

    public Guid ProductsId
    {
        get => _productsId;
        set => SetProperty(ref _productsId, value);
    }

    public virtual Product Products
    {
        get => _products;
        set => SetProperty(ref _products, value);
    }

    public Guid TransactionId
    {
        get => _transactionId;
        set => SetProperty(ref _transactionId, value);
    }

    public virtual Transaction Transaction
    {
        get => _transaction;
        set => SetProperty(ref _transaction, value);
    }

    public int? Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }
}