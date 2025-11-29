namespace NppAccessControl.DAL.Entities.Base;

public abstract record BaseEntity
{
    public Guid Id { get; init; }
}
