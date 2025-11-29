namespace NppAccessControl.DAL.Entities.Base;

public abstract record Person : BaseEntity
{
    public required string FullName { get; init; }

    public DateTime BirthDate { get; init; }

    public required string Phone { get; init; }
}
