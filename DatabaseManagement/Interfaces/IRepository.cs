namespace DatabaseManagement.Interfaces;

public interface IRepository<T> 
    where T : Entity
{
    IEnumerable<T> GetAll();
    T GetById(Guid id);
    bool Add(T entity);
    bool Update(T entity);
    bool Delete(T entity);
}