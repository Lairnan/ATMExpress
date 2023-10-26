﻿namespace DatabaseManagement.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? FindById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
}