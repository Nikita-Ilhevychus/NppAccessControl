using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record Visitor : Person
{
    public required string VisitPurpose { get; init; }

    public DateTime PlannedVisitDate { get; init; }
}
