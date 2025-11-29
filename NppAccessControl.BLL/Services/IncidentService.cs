using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services.Interfaces;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Services;

public class IncidentService(IUnitOfWork unitOfWork) : IIncidentService
{
    public async Task<Incident?> CreateIncidentForDeniedAccessAsync(
        AccessDecisionResult decision,
        PassageEvent passageEvent,
        string reason,
        UserAccount createdBy,
        CancellationToken cancellationToken = default)
    {
        if (decision.Result == AccessResult.Allowed)
        {
            return null;
        }

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            Title = "Access denied",
            Description = reason,
            Severity = IncidentSeverity.Medium,
            Status = IncidentStatus.Open,
            CreatedAt = DateTime.UtcNow,
            ResolvedAt = DateTime.MinValue,
            SourceEvent = passageEvent,
            CreatedBy = createdBy,
            ResolvedBy = createdBy
        };

        await unitOfWork.Incidents.AddAsync(incident, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return incident;
    }
}
