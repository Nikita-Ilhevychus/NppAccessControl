using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Base;

namespace NppAccessControl.DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AccessControlSystem> AccessControlSystems => Set<AccessControlSystem>();
    public DbSet<AccessCard> AccessCards => Set<AccessCard>();
    public DbSet<AccessPermission> AccessPermissions => Set<AccessPermission>();
    public DbSet<AccessProfile> AccessProfiles => Set<AccessProfile>();
    public DbSet<AccessZone> AccessZones => Set<AccessZone>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<SystemRole> SystemRoles => Set<SystemRole>();
    public DbSet<SystemPermission> SystemPermissions => Set<SystemPermission>();
    public DbSet<PermissionCode> PermissionCodes => Set<PermissionCode>();
    public DbSet<PassageEvent> PassageEvents => Set<PassageEvent>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Contractor> Contractors => Set<Contractor>();
    public DbSet<Checkpoint> Checkpoints => Set<Checkpoint>();
    public DbSet<CardReader> CardReaders => Set<CardReader>();
    public DbSet<BiometricScanner> BiometricScanners => Set<BiometricScanner>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Device> Devices => Set<Device>();
}
