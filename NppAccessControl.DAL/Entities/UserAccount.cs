using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record UserAccount : BaseEntity
{
    public required string Login { get; init; }

    public required string PasswordHash { get; init; }

    public bool IsActive { get; init; }

    public required Employee Owner { get; init; }

    public List<SystemRole> Roles { get; init; } = [];
}
