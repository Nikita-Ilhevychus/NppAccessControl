using NppAccessControl.DAL.Entities.Base;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record AccessControlSystem : BaseEntity
{
    public required string Name { get; init; }

    public EmergencyMode EmergencyMode { get; init; }

    public List<Checkpoint> Checkpoints { get; init; } = [];

    public List<AccessZone> Zones { get; init; } = [];

    public List<PassageEvent> Events { get; init; } = [];

    public List<Incident> Incidents { get; init; } = [];
}
