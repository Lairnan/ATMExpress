using IIC.Interfaces;
using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class TransactionRepository : IRepository<Transaction>
{
    private readonly DatabaseManagementContext _dbContext;
    
    public TransactionRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Transaction> GetAll()
        => _dbContext.Transactions
            .Include(s => s.Card)
            .Include(s => s.ProductTransactions)
            .AsEnumerable();

    public Transaction? FindById(Guid id)
    {
        var cards = _dbContext.Transactions
            .Include(s => s.Card)
            .Include(s => s.ProductTransactions);
        return cards.FirstOrDefault(s => s.Id.Equals(id));
    }

    public void Add(Transaction entity)
    {
        if (_dbContext.Transactions.Any(s => s.Id.Equals(entity))) throw new InvalidOperationException("Element already exists");
        if (entity.Card == null) throw new InvalidOperationException("Card can't be empty");
        
        entity.DateCreated = DateTime.Now;
        _dbContext.Transactions.Add(entity);
    }

    public void Update(Transaction entity)
    {
        var ent = _dbContext.Transactions
            .Include(s => s.Card)
            .Include(s => s.ProductTransactions)
            .FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element not found");
        if (entity.Card == null) throw new InvalidOperationException("Card can't be empty");

        ent.Card = entity.Card;
        ent.CardId = entity.CardId;
        ent.ProductTransactions = entity.ProductTransactions;
        ent.Value = entity.Value;
        ent.TransactionType = entity.TransactionType;
    }

    public void Delete(Transaction entity)
    {
        var ent = _dbContext.Transactions.FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element already deleted");

        _dbContext.Transactions.Remove(ent);
    }

    public async void Save() 
        => await _dbContext.SaveChangesAsync();
}