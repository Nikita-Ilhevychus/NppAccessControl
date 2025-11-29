using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Interfaces.Base;

namespace NppAccessControl.DAL.Repositories.Interfaces;

public interface ICheckpointRepository : IBaseRepository<Checkpoint>
{
    Task<Checkpoint?> GetByIdWithZonesAndDevicesAsync(Guid id, CancellationToken cancellationToken = default);
}
