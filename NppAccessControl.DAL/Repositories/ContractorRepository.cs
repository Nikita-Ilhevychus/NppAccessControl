using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class ContractorRepository(AppDbContext context)
    : BaseRepository<Contractor>(context), IContractorRepository;
