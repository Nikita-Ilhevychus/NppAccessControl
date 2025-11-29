using NppAccessControl.DAL.Entities.Base;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record AccessCard : BaseEntity
{
    public required string CardNumber { get; init; }

    public CardType Type { get; init; }

    public CardStatus Status { get; init; }

    public DateTime IssueDate { get; init; }

    public DateTime ExpiryDate { get; init; }

    public required Person Owner { get; init; }

    public required AccessProfile Profile { get; init; }
}
