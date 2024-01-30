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

    public int GetCount(int? page = null, int? pageSize = null)
    {
        if (page == null || pageSize == null)
            return _dbContext.Transactions.Count();
        
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 40 : pageSize;

        return _dbContext.Transactions
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .Count();
    }
        
    public IEnumerable<Transaction> GetAll(int page = 1, int pageSize = 40) => _dbContext.Transactions
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Include(t => t.Card)
        .AsEnumerable();
        
    public Transaction? FindById(Guid id) => _dbContext.Transactions
        .Include(t => t.Card)
        .FirstOrDefault(t => t.Id == id);
        
    public async Task AddAsync(Transaction entity)
    {
        try
        {
            if (_dbContext.Transactions.Any(t => t.Id == entity.Id))
                throw new InvalidOperationException("Transaction with the same ID already exists");
                
            if (entity.Card == null)
                throw new InvalidOperationException("Card can't be empty");
                
            entity.DateCreated = DateTime.Now;
            _dbContext.Transactions.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while adding transaction to the database", ex);
        }
    }

    public async Task UpdateAsync(Transaction entity)
    {
        try
        {
            var existingTransaction = _dbContext.Transactions
                .Include(t => t.Card)
                .Include(t => t.ProductTransactions)
                .FirstOrDefault(t => t.Id == entity.Id);
                
            if (existingTransaction == null)
                throw new InvalidOperationException("Transaction not found");
                
            if (entity.Card == null)
                throw new InvalidOperationException("Card can't be empty");
                
            existingTransaction.Card = entity.Card;
            existingTransaction.CardId = entity.CardId;
            existingTransaction.ProductTransactions = entity.ProductTransactions;
            existingTransaction.Value = entity.Value;
            existingTransaction.TransactionType = entity.TransactionType;
                
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while updating transaction in the database", ex);
        }
    }

    public async Task DeleteAsync(Transaction entity)
    {
        try
        {
            var existingTransaction = _dbContext.Transactions.FirstOrDefault(t => t.Id == entity.Id);
                
            if (existingTransaction == null)
                throw new InvalidOperationException("Transaction not found");
                
            _dbContext.Transactions.Remove(existingTransaction);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while deleting transaction from the database", ex);
        }
    }
    
    public IEnumerable<ProductTransaction> GetProductTransactionsForTransaction(Guid transactionId) =>
        _dbContext.ProductTransactions.Where(pt => pt.TransactionId == transactionId).ToList();
}