using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public abstract record Device : BaseEntity
{
    public required string SerialNumber { get; init; }

    public required string Model { get; init; }

    public DateTime InstalledAt { get; init; }

    public required Checkpoint Checkpoint { get; init; }
}
