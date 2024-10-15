namespace BitFinance.Data.Repositories;

public interface IRepository<T, TId> 
    where TId : IEquatable<TId> 
    where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(TId id);
    Task<T> CreateAsync(T obj);
    Task UpdateAsync(T obj);
    Task DeleteAsync(T obj);
}