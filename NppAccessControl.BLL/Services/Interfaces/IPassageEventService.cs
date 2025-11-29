using NppAccessControl.BLL.Models;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.BLL.Services.Interfaces;

public interface IPassageEventService
{
    Task<PassageEvent> RegisterPassageAsync(
        AccessDecisionResult decision,
        Device device,
        UserAccount processedBy,
        AccessResult resultOverride,
        string reason,
        CancellationToken cancellationToken = default);
}
