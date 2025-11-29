using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Tests.Repositories.Support;

namespace NppAccessControl.DAL.Tests.Repositories;

public class CheckpointRepositoryTests
{
    [Test]
    public async Task GetByIdWithZonesAndDevicesAsync_ReturnsCheckpointWithDetails()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByIdWithZonesAndDevicesAsync_ReturnsCheckpointWithDetails));

        var checkpoint = new Checkpoint
        {
            Id = Guid.NewGuid(),
            Code = "CP1",
            Name = "Gate 1",
            Location = "North",
            IsActive = true,
            Zones = [],
            Devices = []
        };

        var zone = new AccessZone
        {
            Id = Guid.NewGuid(),
            Code = "Z1",
            Name = "Zone 1",
            SecurityLevel = SecurityLevel.High
        };

        var device = new CardReader
        {
            Id = Guid.NewGuid(),
            SerialNumber = "SN-123",
            Model = "CR-900",
            InstalledAt = DateTime.UtcNow,
            Checkpoint = checkpoint,
            InterfaceType = ReaderInterfaceType.Ethernet
        };

        checkpoint.Zones.Add(zone);
        checkpoint.Devices.Add(device);

        await context.AddRangeAsync(zone, device, checkpoint);
        await context.SaveChangesAsync();

        var repository = new CheckpointRepository(context);

        // Act
        var result = await repository.GetByIdWithZonesAndDevicesAsync(checkpoint.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Zones, Has.Count.EqualTo(1));
        Assert.That(result.Devices, Has.Count.EqualTo(1));
        Assert.That(result.Devices[0].Checkpoint.Id, Is.EqualTo(checkpoint.Id));
    }

    [Test]
    public async Task GetByIdWithZonesAndDevicesAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByIdWithZonesAndDevicesAsync_ReturnsNull_WhenNotFound));
        var repository = new CheckpointRepository(context);

        // Act
        var result = await repository.GetByIdWithZonesAndDevicesAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }
}
