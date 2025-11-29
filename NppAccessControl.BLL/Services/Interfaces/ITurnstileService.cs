using NppAccessControl.BLL.Models;
using NppAccessControl.DAL.Entities;

namespace NppAccessControl.BLL.Services.Interfaces;

public interface ITurnstileService
{
    Task<TurnstileProcessResult> ProcessCardReadAsync(
        string cardNumber,
        Guid checkpointId,
        Guid systemId,
        DateTime readTime,
        Device device,
        UserAccount processedBy,
        CancellationToken cancellationToken = default);
}
