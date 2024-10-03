using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class CardRepository : IRepository<Card>
{
    private readonly DatabaseManagementContext _dbContext;

    public CardRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public int GetCount(int? page = null, int? pageSize = null, Func<Card, bool>? predicate = null)
    {
        if (page == null || pageSize == null)
            return _dbContext.Cards.Count();
        
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 40 : pageSize;
        
        return _dbContext.Cards
            .Where(predicate ?? (_ => true))
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .Count();
    }
    
    public IEnumerable<Card> GetAll(int page = 1, int pageSize = 40, Func<Card, bool>? predicate = null) => _dbContext.Cards
        .Include(c => c.User)
        .Where(predicate ?? (_ => true))
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsEnumerable();

    public Card? FindById(Guid id) => _dbContext.Cards
        .Include(c => c.User)
        .FirstOrDefault(c => c.Id == id);

    public async Task AddAsync(Card entity)
    {
        try
        {
            if (_dbContext.Cards.Any(c => c.Id == entity.Id))
                throw new InvalidOperationException("Card with the same ID already exists");

            if (!string.IsNullOrWhiteSpace(entity.CardNumber) && entity.CardNumber.Length != 16)
                throw new InvalidOperationException("CardNumber should be equal to 16");

            if (!string.IsNullOrWhiteSpace(entity.CardNumber) && _dbContext.Cards.Any(c => string.Equals(c.CardNumber.Trim(), entity.CardNumber.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                throw new InvalidOperationException("CardNumber already exists");

            if (entity.Balance < 0m)
                throw new InvalidOperationException("Balance can't be less than 0");

            entity.CardNumber ??= GenerateCardNumber();

            _dbContext.Cards.Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while adding card to the database", ex);
        }
    }

    public async Task UpdateAsync(Card entity)
    {
        try
        {
            var existingCard = _dbContext.Cards
                .Include(c => c.User)
                .Include(c => c.Transactions)
                .FirstOrDefault(c => c.Id == entity.Id);

            if (existingCard == null)
                throw new InvalidOperationException("Card not found");

            if (!string.IsNullOrWhiteSpace(entity.CardNumber) && entity.CardNumber.Length != 16)
                throw new InvalidOperationException("CardNumber should be equal to 16");

            if (entity.Balance < 0m)
                throw new InvalidOperationException("Balance can't be less than 0");

            existingCard.CardNumber = string.IsNullOrWhiteSpace(entity.CardNumber) ? GenerateCardNumber() : entity.CardNumber;
            existingCard.Balance = entity.Balance;
            existingCard.Cardless = entity.Cardless;
            existingCard.User = entity.User;
            existingCard.UserId = entity.UserId;
            existingCard.Transactions = entity.Transactions;

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while updating card in the database", ex);
        }
    }

    public async Task DeleteAsync(Card entity)
    {
        try
        {
            var existingCard = _dbContext.Cards.FirstOrDefault(c => c.Id == entity.Id);

            if (existingCard == null)
                throw new InvalidOperationException("Card already deleted");

            _dbContext.Cards.Remove(existingCard);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while deleting card from the database", ex);
        }
    }

    public IEnumerable<Transaction> GetTransactionsForCard(Guid cardId) =>
        _dbContext.Transactions.Where(t => t.CardId == cardId).ToList();

    private static string GenerateCardNumber()
    {
        var random = new Random();
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}