using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        AccessControlSystems = new AccessControlSystemRepository(context);
        AccessCards = new AccessCardRepository(context);
        AccessPermissions = new AccessPermissionRepository(context);
        AccessProfiles = new AccessProfileRepository(context);
        AccessZones = new AccessZoneRepository(context);
        Visitors = new VisitorRepository(context);
        UserAccounts = new UserAccountRepository(context);
        SystemRoles = new SystemRoleRepository(context);
        SystemPermissions = new SystemPermissionRepository(context);
        PermissionCodes = new PermissionCodeRepository(context);
        PassageEvents = new PassageEventRepository(context);
        Incidents = new IncidentRepository(context);
        Employees = new EmployeeRepository(context);
        Contractors = new ContractorRepository(context);
        Checkpoints = new CheckpointRepository(context);
        CardReaders = new CardReaderRepository(context);
        BiometricScanners = new BiometricScannerRepository(context);
    }

    public IAccessControlSystemRepository AccessControlSystems { get; }
    public IAccessCardRepository AccessCards { get; }
    public IAccessPermissionRepository AccessPermissions { get; }
    public IAccessProfileRepository AccessProfiles { get; }
    public IAccessZoneRepository AccessZones { get; }
    public IVisitorRepository Visitors { get; }
    public IUserAccountRepository UserAccounts { get; }
    public ISystemRoleRepository SystemRoles { get; }
    public ISystemPermissionRepository SystemPermissions { get; }
    public IPermissionCodeRepository PermissionCodes { get; }
    public IPassageEventRepository PassageEvents { get; }
    public IIncidentRepository Incidents { get; }
    public IEmployeeRepository Employees { get; }
    public IContractorRepository Contractors { get; }
    public ICheckpointRepository Checkpoints { get; }
    public ICardReaderRepository CardReaders { get; }
    public IBiometricScannerRepository BiometricScanners { get; }

    public int SaveChanges() => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _context.DisposeAsync();
    }
}
