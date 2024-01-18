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

    public IEnumerable<Product> GetAll() => _dbContext.Products
    public IEnumerable<Product> GetAll(int page = 1, int pageSize = 40) => _dbContext.Products
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsEnumerable();

    public Product? FindById(Guid id) => _dbContext.Products
        .FirstOrDefault(p => p.Id == id);

    public async Task AddAsync(Product entity)
    {
        try
        {
            if (_dbContext.Products.Any(p => p.Id == entity.Id))
                throw new InvalidOperationException("Product with the same ID already exists");

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new ArgumentNullException(nameof(entity.Name), "Name can't be empty");

            if (string.IsNullOrWhiteSpace(entity.Description))
                throw new ArgumentNullException(nameof(entity.Description), "Description can't be empty");

            if (entity.Weight < 0m)
                throw new InvalidOperationException("Weight can't be less than 0");

            if (entity.Price < 0m)
                throw new InvalidOperationException("Price can't be less than 0");

            entity.DateCreated = DateTime.Now;
            _dbContext.Products.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while adding product to the database", ex);
        }
    }

    public async Task UpdateAsync(Product entity)
    {
        try
        {
            var existingProduct = _dbContext.Products
                .Include(p => p.Transactions)
                .FirstOrDefault(p => p.Id == entity.Id);

            if (existingProduct == null)
                throw new InvalidOperationException("Product not found");

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new ArgumentNullException(nameof(entity.Name), "Name can't be empty");

            if (string.IsNullOrWhiteSpace(entity.Description))
                throw new ArgumentNullException(nameof(entity.Description), "Description can't be empty");

            if (entity.Weight < 0m)
                throw new InvalidOperationException("Weight can't be less than 0");

            if (entity.Price < 0m)
                throw new InvalidOperationException("Price can't be less than 0");

            existingProduct.Name = entity.Name;
            existingProduct.Description = entity.Description;
            existingProduct.Price = entity.Price;
            existingProduct.Weight = entity.Weight;
            existingProduct.DateCreated = entity.DateCreated;
            existingProduct.Transactions = entity.Transactions;

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while updating product in the database", ex);
        }
    }

    public async Task DeleteAsync(Product entity)
    {
        try
        {
            var existingProduct = _dbContext.Products.FirstOrDefault(p => p.Id == entity.Id);

            if (existingProduct == null)
                throw new InvalidOperationException("Product already deleted");

            _dbContext.Products.Remove(existingProduct);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while deleting product from the database", ex);
        }
    }

    public IEnumerable<Transaction> GetTransactionsForProduct(Guid productId) =>
        _dbContext.ProductTransactions
            .Where(pt => pt.ProductsId == productId)
            .Select(pt => pt.Transaction)
            .ToList();
}