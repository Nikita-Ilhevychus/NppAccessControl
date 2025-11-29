using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;
using NppAccessControl.DAL.Repositories.Interfaces;

namespace NppAccessControl.DAL.Repositories;

public class CardReaderRepository(AppDbContext context)
    : BaseRepository<CardReader>(context), ICardReaderRepository;
