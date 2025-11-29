using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class CheckpointRepository(AppDbContext context)
    : BaseRepository<Checkpoint>(context), ICheckpointRepository
{
    public Task<Checkpoint?> GetByIdWithZonesAndDevicesAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        DbSet
            .Include(c => c.Zones)
            .Include(c => c.Devices)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}
