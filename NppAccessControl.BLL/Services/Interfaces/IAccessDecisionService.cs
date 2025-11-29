using NppAccessControl.BLL.Models;

namespace NppAccessControl.BLL.Services.Interfaces;

public interface IAccessDecisionService
{
    Task<AccessDecisionResult> DecideAsync(string cardNumber, Guid checkpointId, Guid systemId, DateTime readTime, CancellationToken cancellationToken = default);
}
