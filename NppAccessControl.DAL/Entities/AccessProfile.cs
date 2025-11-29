using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record AccessProfile : BaseEntity
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public List<AccessPermission> Permissions { get; init; } = [];
}
