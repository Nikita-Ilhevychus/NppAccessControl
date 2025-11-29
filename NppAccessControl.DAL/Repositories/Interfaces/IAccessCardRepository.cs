using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Interfaces.Base;

namespace NppAccessControl.DAL.Repositories.Interfaces;

public interface IAccessCardRepository : IBaseRepository<AccessCard>
{
    Task<AccessCard?> GetByCardNumberWithDetailsAsync(string cardNumber, CancellationToken cancellationToken = default);
}
