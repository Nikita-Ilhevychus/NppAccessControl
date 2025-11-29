using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record Employee : Person
{
    public required string PersonnelNumber { get; init; }

    public required string Department { get; init; }

    public required string Position { get; init; }
}
