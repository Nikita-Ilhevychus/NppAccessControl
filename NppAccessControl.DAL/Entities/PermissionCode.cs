using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record PermissionCode : BaseEntity
{
    public required string Code { get; init; }

    public string? Description { get; init; }
}
