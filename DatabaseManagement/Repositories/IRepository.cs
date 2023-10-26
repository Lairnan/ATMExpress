using CSA;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public interface IRepository<T>
    where T : Entity
{
    IEnumerable<T> GetAll();
    T? FindById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
}