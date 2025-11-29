using Moq;
using NppAccessControl.BLL.Services;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Tests.Services;

public class AccessDecisionServiceTests
{
    [Test]
    public async Task DecideAsync_Allows_WhenPermissionValid()
    {
        // Arrange
        var readTime = new DateTime(2025, 1, 1, 9, 0, 0, DateTimeKind.Utc);
        var zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z1", Name = "Zone 1", SecurityLevel = SecurityLevel.Normal };
        var profile = new AccessProfile { Id = Guid.NewGuid(), Name = "Standard", Permissions = [] };
        var permission = new AccessPermission
        {
            Id = Guid.NewGuid(),
            Profile = profile,
            Zone = zone,
            ValidFrom = readTime.AddDays(-1),
            ValidTo = readTime.AddDays(1),
            TimeFrom = new TimeOnly(8, 0),
            TimeTo = new TimeOnly(18, 0)
        };
        profile.Permissions.Add(permission);

        var card = new AccessCard
        {
            Id = Guid.NewGuid(),
            CardNumber = "C1",
            Type = CardType.Employee,
            Status = CardStatus.Active,
            IssueDate = readTime.AddDays(-10),
            ExpiryDate = readTime.AddDays(10),
            Owner = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Employee",
                BirthDate = new DateTime(1990, 1, 1),
                Phone = "123",
                Department = "IT",
                PersonnelNumber = "1",
                Position = "Dev"
            },
            Profile = profile
        };

        var checkpoint = new Checkpoint
        {
            Id = Guid.NewGuid(),
            Code = "CP1",
            Name = "Gate",
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

        var uowMock = new Mock<IUnitOfWork>();
        var cardRepo = new Mock<IAccessCardRepository>();
        var checkpointRepo = new Mock<ICheckpointRepository>();
        var systemRepo = new Mock<IAccessControlSystemRepository>();

        cardRepo.Setup(r => r.GetByCardNumberWithDetailsAsync("C1", It.IsAny<CancellationToken>())).ReturnsAsync(card);
        checkpointRepo.Setup(r => r.GetByIdWithZonesAndDevicesAsync(checkpoint.Id, It.IsAny<CancellationToken>())).ReturnsAsync(checkpoint);
        systemRepo.Setup(r => r.GetByIdWithTopologyAsync(system.Id, It.IsAny<CancellationToken>())).ReturnsAsync(system);

        uowMock.SetupGet(u => u.AccessCards).Returns(cardRepo.Object);
        uowMock.SetupGet(u => u.Checkpoints).Returns(checkpointRepo.Object);
        uowMock.SetupGet(u => u.AccessControlSystems).Returns(systemRepo.Object);

        var service = new AccessDecisionService(uowMock.Object);

        // Act
        var result = await service.DecideAsync("C1", checkpoint.Id, system.Id, readTime);

        // Assert
        Assert.That(result.Result, Is.EqualTo(AccessResult.Allowed));
        Assert.That(result.GrantedPermission, Is.EqualTo(permission));
    }

    [Test]
    public async Task DecideAsync_Denies_WhenCardInactive()
    {
        // Arrange
        var readTime = DateTime.UtcNow;
        var card = new AccessCard
        {
            Id = Guid.NewGuid(),
            CardNumber = "C1",
            Status = CardStatus.Blocked,
            IssueDate = readTime.AddDays(-1),
            ExpiryDate = readTime.AddDays(1),
            Type = CardType.Employee,
            Owner = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Employee",
                BirthDate = new DateTime(1990, 1, 1),
                Phone = "123",
                Department = "IT",
                PersonnelNumber = "1",
                Position = "Dev"
            },
            Profile = new AccessProfile { Id = Guid.NewGuid(), Name = "Standard" }
        };

        var checkpoint = new Checkpoint { Id = Guid.NewGuid(), Code = "CP1", Name = "Gate", Location = "A", IsActive = true };
        var system = new AccessControlSystem { Id = Guid.NewGuid(), Name = "Sys", EmergencyMode = EmergencyMode.Normal };

        var uowMock = new Mock<IUnitOfWork>();
        var cardRepo = new Mock<IAccessCardRepository>();
        var checkpointRepo = new Mock<ICheckpointRepository>();
        var systemRepo = new Mock<IAccessControlSystemRepository>();

        cardRepo.Setup(r => r.GetByCardNumberWithDetailsAsync("C1", It.IsAny<CancellationToken>())).ReturnsAsync(card);
        checkpointRepo.Setup(r => r.GetByIdWithZonesAndDevicesAsync(checkpoint.Id, It.IsAny<CancellationToken>())).ReturnsAsync(checkpoint);
        systemRepo.Setup(r => r.GetByIdWithTopologyAsync(system.Id, It.IsAny<CancellationToken>())).ReturnsAsync(system);

        uowMock.SetupGet(u => u.AccessCards).Returns(cardRepo.Object);
        uowMock.SetupGet(u => u.Checkpoints).Returns(checkpointRepo.Object);
        uowMock.SetupGet(u => u.AccessControlSystems).Returns(systemRepo.Object);

        var service = new AccessDecisionService(uowMock.Object);

        // Act
        var result = await service.DecideAsync("C1", checkpoint.Id, system.Id, readTime);

        // Assert
        Assert.That(result.Result, Is.EqualTo(AccessResult.Denied));
        Assert.That(result.Reason, Does.Contain("not active"));
    }
}
