using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class AccessControlSystemRepository(AppDbContext context)
    : BaseRepository<AccessControlSystem>(context), IAccessControlSystemRepository
{
    public Task<AccessControlSystem?> GetByIdWithTopologyAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        DbSet
            .Include(s => s.Checkpoints)
            .Include(s => s.Zones)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
}
