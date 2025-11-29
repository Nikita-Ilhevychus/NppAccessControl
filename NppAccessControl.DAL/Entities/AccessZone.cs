using NppAccessControl.DAL.Entities.Base;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record AccessZone : BaseEntity
{
    public required string Code { get; init; }

    public required string Name { get; init; }

    public SecurityLevel SecurityLevel { get; init; }
}
