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

    public IEnumerable<User> GetAll() => _dbContext.Users.ToList();
    public User? FindById(Guid id) => _dbContext.Users.Find(id);

    public void Add(User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "User entity is null");

        if (string.IsNullOrWhiteSpace(entity.Login))
            throw new ArgumentException("Login can't be empty", nameof(entity.Login));

        if (string.IsNullOrWhiteSpace(entity.Password))
            throw new ArgumentException("Password can't be empty", nameof(entity.Password));

        if (_dbContext.Users.Any(u => u.Id == entity.Id))
            throw new InvalidOperationException("User with the same ID already exists");

        if (_dbContext.Users.Any(u => string.Equals(u.Login.Trim(), entity.Login.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException("User with the same login already exists");

        entity.DateCreated = DateTime.Now;
        _dbContext.Users.Add(entity);
    }

    public void Update(User entity)
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
    }

    public void Delete(User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "User entity is null");

        var existingUser = _dbContext.Users.Find(entity.Id);

        if (existingUser == null)
            throw new InvalidOperationException("User not found");

        _dbContext.Users.Remove(existingUser);
    }

    public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    public IEnumerable<Card> GetCardsForUser(Guid userId) => _dbContext.Cards.Where(c => c.UserId == userId).ToList();

}