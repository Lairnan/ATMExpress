using IIC.Interfaces;
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

    public IEnumerable<User> GetAll()
        => _dbContext.Users.Include(s => s.Cards).AsEnumerable();

    public User? FindById(Guid id)
    {
        var users = _dbContext.Users.Include(s => s.Cards);
        return users.FirstOrDefault(s => s.Id.Equals(id));
    }

    public void Add(User entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Login)) throw new ArgumentNullException(nameof(entity.Login), "Login can't be empty");
        if (string.IsNullOrWhiteSpace(entity.Password)) throw new ArgumentNullException(nameof(entity.Password), "Password can't be empty");
        if (_dbContext.Users.ToList().Any(s => s.Id.Equals(entity.Id))) throw new InvalidOperationException("Element already exists");
        if (_dbContext.Users.ToList().Any(s => string.Equals(s.Login.Trim(), entity.Login.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException("Login already exists");

        entity.DateCreated = DateTime.Now;
        _dbContext.Users.Add(entity);
    }

    public void Update(User entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Login)) throw new ArgumentNullException(nameof(entity.Login), "Login can't be empty");
        if (string.IsNullOrWhiteSpace(entity.Password)) throw new ArgumentNullException(nameof(entity.Password), "Password can't be empty");
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

        _dbContext.Users.Remove(ent);
    }

    public async void Save() 
        => await _dbContext.SaveChangesAsync();
}