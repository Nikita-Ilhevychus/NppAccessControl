using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class IncidentRepository(AppDbContext context)
    : BaseRepository<Incident>(context), IIncidentRepository;
