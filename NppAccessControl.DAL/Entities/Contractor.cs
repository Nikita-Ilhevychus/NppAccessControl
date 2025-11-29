using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL.Entities;

public record Contractor : Person
{
    public required string CompanyName { get; init; }

    public required string ContractNumber { get; init; }

    public DateTime ContractValidTo { get; init; }
}
