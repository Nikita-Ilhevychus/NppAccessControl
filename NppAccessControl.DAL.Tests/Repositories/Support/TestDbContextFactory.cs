using Microsoft.EntityFrameworkCore;
using NppAccessControl.DAL;

namespace NppAccessControl.DAL.Tests.Repositories.Support;

public static class TestDbContextFactory
{
    public static AppDbContext Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new AppDbContext(options);
    }
}
