using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record SystemRole : BaseEntity
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public List<SystemPermission> Permissions { get; init; } = [];
}
