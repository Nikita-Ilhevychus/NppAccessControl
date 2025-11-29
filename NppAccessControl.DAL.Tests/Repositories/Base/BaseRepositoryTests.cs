using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NppAccessControl.DAL;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Repositories.Base;

namespace NppAccessControl.DAL.Tests.Repositories.Base;

public class BaseRepositoryTests
{
    [Test]
    public async Task AddAsync_UsesDbSetAddAsync()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        var contextMock = new Mock<AppDbContext>(options);
        var dbSetMock = new Mock<DbSet<AccessZone>>();

        contextMock.Setup(c => c.Set<AccessZone>()).Returns(dbSetMock.Object);
        dbSetMock
            .Setup(s => s.AddAsync(It.IsAny<AccessZone>(), It.IsAny<CancellationToken>()))
            .Returns((AccessZone entity, CancellationToken _) => ValueTask.FromResult((EntityEntry<AccessZone>)null!));

        var repository = new BaseRepository<AccessZone>(contextMock.Object);
        var zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z1", Name = "Zone 1", SecurityLevel = SecurityLevel.Normal };

        // Act
        await repository.AddAsync(zone);

        // Assert
        dbSetMock.Verify(s => s.AddAsync(zone, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Remove_CallsDbSetRemove()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        var contextMock = new Mock<AppDbContext>(options);
        var dbSetMock = new Mock<DbSet<AccessZone>>();

        contextMock.Setup(c => c.Set<AccessZone>()).Returns(dbSetMock.Object);
        var repository = new BaseRepository<AccessZone>(contextMock.Object);
        var zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z2", Name = "Zone 2", SecurityLevel = SecurityLevel.High };

        // Act
        repository.Remove(zone);

        // Assert
        dbSetMock.Verify(s => s.Remove(zone), Times.Once);
    }

    [Test]
    public void Update_CallsDbSetUpdate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        var contextMock = new Mock<AppDbContext>(options);
        var dbSetMock = new Mock<DbSet<AccessZone>>();

        contextMock.Setup(c => c.Set<AccessZone>()).Returns(dbSetMock.Object);
        var repository = new BaseRepository<AccessZone>(contextMock.Object);
        var zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z3", Name = "Zone 3", SecurityLevel = SecurityLevel.Critical };

        // Act
        repository.Update(zone);

        // Assert
        dbSetMock.Verify(s => s.Update(zone), Times.Once);
    }
}
