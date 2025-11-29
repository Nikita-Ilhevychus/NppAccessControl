using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Repositories.Interfaces.Base;

public interface IBaseRepository<T> where T : BaseEntity
{
    IQueryable<T> Query();

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Remove(T entity);
}
