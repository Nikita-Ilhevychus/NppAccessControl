using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class EmployeeRepository(AppDbContext context)
    : BaseRepository<Employee>(context), IEmployeeRepository;
