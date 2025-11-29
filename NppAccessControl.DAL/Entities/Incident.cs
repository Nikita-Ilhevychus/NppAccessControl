using NppAccessControl.DAL.Entities.Base;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record Incident : BaseEntity
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public IncidentSeverity Severity { get; init; }

    public IncidentStatus Status { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime ResolvedAt { get; init; }

    public required PassageEvent SourceEvent { get; init; }

    public required UserAccount CreatedBy { get; init; }

    public required UserAccount ResolvedBy { get; init; }
}
