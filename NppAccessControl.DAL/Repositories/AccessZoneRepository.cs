using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class AccessZoneRepository(AppDbContext context)
    : BaseRepository<AccessZone>(context), IAccessZoneRepository;
