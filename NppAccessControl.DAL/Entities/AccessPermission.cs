using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record AccessPermission : BaseEntity
{
    public required AccessProfile Profile { get; init; }

    public required AccessZone Zone { get; init; }

    public DateTime ValidFrom { get; init; }

    public DateTime ValidTo { get; init; }

    public TimeOnly TimeFrom { get; init; }

    public TimeOnly TimeTo { get; init; }
}
