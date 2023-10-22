using DatabaseManagement.Entities;
using DatabaseManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly DatabaseManagementContext _dbContext;
    
    public UserRepository(DatabaseManagementContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<User> GetAll()
        => _dbContext.Users.Include(s => s.Cards).AsEnumerable();

    public User? GetById(Guid id)
    {
        var users = _dbContext.Users.Include(s => s.Cards);
        return users.FirstOrDefault(s => s.Id.Equals(id));
    }

    public void Add(User entity)
    {
        if (_dbContext.Users.Any(s => s.Id.Equals(entity))) throw new InvalidOperationException("Element already exists");
        if (_dbContext.Users.Any(s => string.Equals(s.Login.Trim(), entity.Login.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException("Login already exists");
        
        _dbContext.Users.Add(entity);
    }

    public void Update(User entity)
    {
        var ent = _dbContext.Users.Include(s => s.Cards).FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element not found");

        ent.Login = entity.Login;
        ent.Password = entity.Password;
        ent.Cards = entity.Cards;
    }

    public void Delete(User entity)
    {
        var ent = _dbContext.Users.FirstOrDefault(s => s.Id.Equals(entity.Id));
        if (ent == null) throw new ArgumentNullException(nameof(ent), "Element already deleted");

        _dbContext.Remove(ent);
    }
}