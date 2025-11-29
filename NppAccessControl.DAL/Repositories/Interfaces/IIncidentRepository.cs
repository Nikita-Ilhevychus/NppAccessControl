using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Interfaces.Base;

namespace NppAccessControl.DAL.Repositories.Interfaces;

public interface IIncidentRepository : IBaseRepository<Incident>
{
    Task<List<Incident>> GetOpenByCardAsync(Guid cardId, CancellationToken cancellationToken = default);
}
