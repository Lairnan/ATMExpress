namespace ESX.Entities
{
    public class ProductTransaction : Entity
    {
        private Guid _productsId;
        private Guid _transactionsId;
        private int? _quantity;
        private Product _products = null!;
        private Transaction _transactions = null!;

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

        public Guid TransactionsId
        {
            get => _transactionsId;
            set => SetProperty(ref _transactionsId, value);
        }

        public virtual Transaction Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        public int? Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
    }
}