namespace BitFinance.API.Repositories;

public interface IRepository<T> 
    where T : class
{
    IEnumerable<T> GetAll();
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);
    T Add();
}