using NppAccessControl.DAL.Entities.Base;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record PassageEvent : BaseEntity
{
    public DateTime EventTime { get; init; }

    public PassageDirection Direction { get; init; }

    public AccessResult Result { get; init; }

    public string? Reason { get; init; }

    public EmergencyMode EmergencyMode { get; init; }

    public required AccessCard Card { get; init; }

    public required Person Person { get; init; }

    public required Checkpoint Checkpoint { get; init; }

    public required AccessZone Zone { get; init; }

    public required Device Device { get; init; }

    public required UserAccount ProcessedBy { get; init; }
}
