using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly DatabaseManagementContext _dbContext;
    
    public ProductRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Product> GetAll()
        => _dbContext.Products
            .Include(s => s.Transactions)
            .AsEnumerable();

    public Product? FindById(Guid id)
    {
        var cards = _dbContext.Products
            .Include(s => s.Transactions);
        return cards.FirstOrDefault(s => s.Id.Equals(id));
    }

    public void Add(Product entity)
    {
        if (_dbContext.Products.Any(s => s.Id.Equals(entity))) throw new InvalidOperationException("Element already exists");
        if (string.IsNullOrWhiteSpace(entity.Name)) throw new ArgumentNullException(nameof(entity.Name), "Name can't be empty");
        if (string.IsNullOrWhiteSpace(entity.Description)) throw new ArgumentNullException(nameof(entity.Description), "Description can't be empty");
        if (entity.Weight < 0m) throw new InvalidOperationException("Weight can't less 0");
        if (entity.Price < 0m) throw new InvalidOperationException("Price can't less 0");

        entity.DateCreated = DateTime.Now;
        _dbContext.Products.Add(entity);
    }

    public void Update(Product entity)
    {
        var ent = _dbContext.Products
            .Include(s => s.Transactions)
            .FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element not found");
        if (string.IsNullOrWhiteSpace(entity.Name)) throw new ArgumentNullException(nameof(entity.Name), "Name can't be empty");
        if (string.IsNullOrWhiteSpace(entity.Description)) throw new ArgumentNullException(nameof(entity.Description), "Description can't be empty");
        if (entity.Weight < 0m) throw new InvalidOperationException("Weight can't less 0");
        if (entity.Price < 0m) throw new InvalidOperationException("Price can't less 0");

        ent.Name = entity.Name;
        ent.Description = entity.Description;
        ent.Price = entity.Price;
        ent.Weight = entity.Weight;
        ent.DateCreated = entity.DateCreated;
        ent.Transactions = entity.Transactions;
    }

    public void Delete(Product entity)
    {
        var ent = _dbContext.Products.FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element already deleted");

        _dbContext.Products.Remove(ent);
    }

    public async void Save() 
        => await _dbContext.SaveChangesAsync();
}