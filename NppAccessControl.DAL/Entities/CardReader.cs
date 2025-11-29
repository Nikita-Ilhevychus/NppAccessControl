using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record CardReader : Device
{
    public ReaderInterfaceType InterfaceType { get; init; }
}
