using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class BiometricScannerRepository(AppDbContext context)
    : BaseRepository<BiometricScanner>(context), IBiometricScannerRepository;
