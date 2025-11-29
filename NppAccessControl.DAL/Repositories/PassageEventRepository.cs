using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class PassageEventRepository(AppDbContext context)
    : BaseRepository<PassageEvent>(context), IPassageEventRepository
{
    public Task<List<PassageEvent>> GetForCardAsync(
        Guid cardId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default) =>
        DbSet
            .Include(e => e.Card)
            .Include(e => e.Checkpoint)
            .Include(e => e.Zone)
            .Where(e => e.Card.Id == cardId && e.EventTime >= from && e.EventTime <= to)
            .OrderBy(e => e.EventTime)
            .ToListAsync(cancellationToken);
}
