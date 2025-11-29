using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Interfaces.Base;

namespace NppAccessControl.DAL.Repositories.Interfaces;

public interface IPassageEventRepository : IBaseRepository<PassageEvent>
{
    Task<List<PassageEvent>> GetForCardAsync(Guid cardId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
}
