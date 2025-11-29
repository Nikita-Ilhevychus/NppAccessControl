using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.DAL.Entities;

public record BiometricScanner : Device
{
    public BiometricType BiometricType { get; init; }
}
