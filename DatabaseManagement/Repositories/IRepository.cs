using CSA;

namespace DatabaseManagement.Repositories;

public interface IRepository<T>
    where T : Entity
{
    IEnumerable<T> GetAll(int page = 1, int pageSize = 40, Func<T, bool>? predicate = null);
    T? FindById(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    int GetCount(int? page = null, int? pageSize = null, Func<T, bool>? predicate = null);
}