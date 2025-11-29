using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Tests.Repositories.Support;

namespace NppAccessControl.DAL.Tests.Repositories;

public class AccessControlSystemRepositoryTests
{
    [Test]
    public async Task GetByIdWithTopologyAsync_ReturnsSystemWithZonesAndCheckpoints()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByIdWithTopologyAsync_ReturnsSystemWithZonesAndCheckpoints));

        var zone = new AccessZone
        {
            Id = Guid.NewGuid(),
            Code = "Z1",
            Name = "Zone 1",
            SecurityLevel = SecurityLevel.Normal
        };

        var checkpoint = new Checkpoint
        {
            Id = Guid.NewGuid(),
            Code = "CP1",
            Name = "Main gate",
            Location = "A",
            IsActive = true,
            Zones = [zone]
        };

        var system = new AccessControlSystem
        {
            Id = Guid.NewGuid(),
            Name = "HQ",
            EmergencyMode = EmergencyMode.Normal,
            Checkpoints = [checkpoint],
            Zones = [zone]
        };

        await context.AddRangeAsync(zone, checkpoint, system);
        await context.SaveChangesAsync();

        var repository = new AccessControlSystemRepository(context);

        // Act
        var result = await repository.GetByIdWithTopologyAsync(system.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Checkpoints, Has.Count.EqualTo(1));
        Assert.That(result.Zones, Has.Count.EqualTo(1));
        Assert.That(result.Checkpoints[0].Code, Is.EqualTo(checkpoint.Code));
    }

    [Test]
    public async Task GetByIdWithTopologyAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByIdWithTopologyAsync_ReturnsNull_WhenNotFound));
        var repository = new AccessControlSystemRepository(context);

        // Act
        var result = await repository.GetByIdWithTopologyAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }
}
