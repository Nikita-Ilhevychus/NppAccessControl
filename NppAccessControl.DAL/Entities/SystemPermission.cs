using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record SystemPermission : BaseEntity
{
    public required PermissionCode PermissionCode { get; init; }
}
