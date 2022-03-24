namespace Case.CoreFunctionality.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetLast24HoursAsync(CancellationToken cancellationToken = default);
}