using DatabaseManagement.Interfaces;

namespace DatabaseManagement.Repositories;

public class Repository<T> : IRepository<T>
    where T : Entity
{
    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Add(T entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(T entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(T entity)
    {
        throw new NotImplementedException();
    }
}