namespace Case.CoreFunctionality.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> SelectAllAsync(CancellationToken cancellationToken = default);
}