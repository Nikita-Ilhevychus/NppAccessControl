using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class AccessCardRepository(AppDbContext context)
    : BaseRepository<AccessCard>(context), IAccessCardRepository
{
    public Task<AccessCard?> GetByCardNumberWithDetailsAsync(
        string cardNumber,
        CancellationToken cancellationToken = default) =>
        DbSet
            .Include(c => c.Owner)
            .Include(c => c.Profile)
            .ThenInclude(p => p.Permissions)
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber, cancellationToken);
}
