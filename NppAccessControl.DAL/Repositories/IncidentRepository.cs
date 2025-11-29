using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class IncidentRepository(AppDbContext context)
    : BaseRepository<Incident>(context), IIncidentRepository
{
    public Task<List<Incident>> GetOpenByCardAsync(
        Guid cardId,
        CancellationToken cancellationToken = default) =>
        DbSet
            .Include(i => i.SourceEvent)
            .ThenInclude(e => e.Card)
            .Where(i => i.SourceEvent.Card.Id == cardId &&
                        (i.Status == IncidentStatus.Open || i.Status == IncidentStatus.InProgress))
            .ToListAsync(cancellationToken);
}
