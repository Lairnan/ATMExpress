using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly DatabaseManagementContext _dbContext;
        
        public TransactionRepository(DatabaseManagementContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<Transaction> GetAll() => _dbContext.Transactions
            .Include(t => t.Card)
            .AsEnumerable();
        
        public Transaction? FindById(Guid id) => _dbContext.Transactions
            .Include(t => t.Card)
            .FirstOrDefault(t => t.Id == id);
        
        public void Add(Transaction entity)
        {
            if (_dbContext.Transactions.Any(t => t.Id == entity.Id))
                throw new InvalidOperationException("Transaction with the same ID already exists");
            
            if (entity.Card == null)
                throw new InvalidOperationException("Card can't be empty");
            
            entity.DateCreated = DateTime.Now;
            _dbContext.Transactions.Add(entity);
        }
        
        public void Update(Transaction entity)
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
        }
        
        public void Delete(Transaction entity)
        {
            var existingTransaction = _dbContext.Transactions.FirstOrDefault(t => t.Id == entity.Id);
            
            if (existingTransaction == null)
                throw new InvalidOperationException("Transaction already deleted");
            
            _dbContext.Transactions.Remove(existingTransaction);
        }
        
        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
        
        public IEnumerable<ProductTransaction> GetProductTransactionsForTransaction(Guid transactionId) =>
            _dbContext.ProductTransactions.Where(pt => pt.TransactionsId == transactionId).ToList();
    }
}
