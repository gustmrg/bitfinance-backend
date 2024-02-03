namespace BitFinance.API.Repositories;

public interface IRepository<T, TId> 
    where TId : IEquatable<TId> 
    where T : class
{
    IEnumerable<T> GetAll();
    Task<T?> GetByIdAsync(TId id);
    Task<T> CreateAsync(T obj);
    Task DeleteAsync(T obj);
}