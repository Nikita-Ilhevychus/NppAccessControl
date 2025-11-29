using NppAccessControl.BLL.Models;
using NppAccessControl.DAL.Entities;

namespace NppAccessControl.BLL.Services.Interfaces;

public interface IIncidentService
{
    Task<Incident?> CreateIncidentForDeniedAccessAsync(
        AccessDecisionResult decision,
        PassageEvent passageEvent,
        string reason,
        UserAccount createdBy,
        CancellationToken cancellationToken = default);
}
