using CSA;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManagement.Repositories;

public interface IRepository<T>
    where T : Entity
{
    IEnumerable<T> GetAll();
    T? FindById(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}