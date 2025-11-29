using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.UnitOfWork.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IAccessControlSystemRepository AccessControlSystems { get; }
    IAccessCardRepository AccessCards { get; }
    IAccessPermissionRepository AccessPermissions { get; }
    IAccessProfileRepository AccessProfiles { get; }
    IAccessZoneRepository AccessZones { get; }
    IVisitorRepository Visitors { get; }
    IUserAccountRepository UserAccounts { get; }
    ISystemRoleRepository SystemRoles { get; }
    ISystemPermissionRepository SystemPermissions { get; }
    IPermissionCodeRepository PermissionCodes { get; }
    IPassageEventRepository PassageEvents { get; }
    IIncidentRepository Incidents { get; }
    IEmployeeRepository Employees { get; }
    IContractorRepository Contractors { get; }
    ICheckpointRepository Checkpoints { get; }
    ICardReaderRepository CardReaders { get; }
    IBiometricScannerRepository BiometricScanners { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
