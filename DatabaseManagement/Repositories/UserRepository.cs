using CSA.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly DatabaseManagementContext _dbContext;
    
    public UserRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int GetCount(int? page = null, int? pageSize = null)
    {
        if (page == null || pageSize == null)
            return _dbContext.Users.Count();
        
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 40 : pageSize;

        return _dbContext.Users
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .Count();
    }

    public IEnumerable<User> GetAll(int page = 1, int pageSize = 40) => _dbContext.Users
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    
    public User? FindById(Guid id) => _dbContext.Users.Find(id);

    public async Task AddAsync(User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "User entity is null");

        if (string.IsNullOrWhiteSpace(entity.Login))
            throw new ArgumentException("Login can't be empty");

        if (string.IsNullOrWhiteSpace(entity.Password))
            throw new ArgumentException("Password can't be empty");
        
        if (_dbContext.Users.ToList().Any(u => u.Id == entity.Id))
            throw new InvalidOperationException("User with the same ID already exists");

        if (_dbContext.Users.ToList().Any(u => string.Equals(u.Login.Trim(), entity.Login.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException("User with the same login already exists");

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            entity.DateCreated = DateTime.Now;
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error while adding user to the database", ex);
        }
    }

    public async Task UpdateAsync(User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "User entity is null");

        var existingUser = _dbContext.Users
            .Include(u => u.Cards)
            .SingleOrDefault(u => u.Id == entity.Id);

        if (existingUser == null)
            throw new InvalidOperationException("User not found");

        existingUser.Login = entity.Login;
        existingUser.Password = entity.Password;
        existingUser.Cards = entity.Cards;

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error while updating user in the database", ex);
        }
    }

    public async Task DeleteAsync(User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "User entity is null");

        var existingUser = await _dbContext.Users.FindAsync(entity.Id);

        if (existingUser == null)
            throw new InvalidOperationException("User not found");

        _dbContext.Users.Remove(existingUser);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Error while deleting user from the database", ex);
        }
    }

    public IEnumerable<Card> GetCardsByUser(Guid userId) => _dbContext.Cards.Where(c => c.UserId == userId).ToList();

}