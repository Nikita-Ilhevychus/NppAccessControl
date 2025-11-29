using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record Checkpoint : BaseEntity
{
    public required string Code { get; init; }

    public required string Name { get; init; }

    public required string Location { get; init; }

    public bool IsActive { get; init; }

    public List<AccessZone> Zones { get; init; } = [];

    public List<Device> Devices { get; init; } = [];
}
