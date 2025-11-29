using Moq;
using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Tests.Services;

public class PassageEventServiceTests
{
    [Test]
    public async Task RegisterPassageAsync_PersistsEvent()
    {
        // Arrange
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FullName = "Emp",
            BirthDate = new DateTime(1990, 1, 1),
            Phone = "123",
            Department = "IT",
            PersonnelNumber = "1",
            Position = "Dev"
        };

        var card = new AccessCard
        {
            Id = Guid.NewGuid(),
            CardNumber = "C1",
            Type = CardType.Employee,
            Status = CardStatus.Active,
            IssueDate = DateTime.UtcNow.AddDays(-1),
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            Owner = employee,
            Profile = new AccessProfile { Id = Guid.NewGuid(), Name = "Default" }
        };

        var checkpoint = new Checkpoint
        {
            Id = Guid.NewGuid(),
            Code = "CP1",
            Name = "Gate",
            Location = "A",
            IsActive = true,
            Zones = [new AccessZone { Id = Guid.NewGuid(), Code = "Z1", Name = "Zone", SecurityLevel = SecurityLevel.Normal }]
        };

        var system = new AccessControlSystem
        {
            Id = Guid.NewGuid(),
            Name = "Sys",
            EmergencyMode = EmergencyMode.Normal
        };

        var context = new AccessDecisionContext(card, checkpoint, system, DateTime.UtcNow);
        var decision = new AccessDecisionResult(context, AccessResult.Allowed, "ok", null);

        var processedBy = new UserAccount
        {
            Id = Guid.NewGuid(),
            Login = "processor",
            PasswordHash = "hash",
            IsActive = true,
            Owner = employee,
            Roles = []
        };

        var device = new CardReader
        {
            Id = Guid.NewGuid(),
            SerialNumber = "SN",
            Model = "M",
            InstalledAt = DateTime.UtcNow,
            Checkpoint = checkpoint,
            InterfaceType = ReaderInterfaceType.Ethernet
        };

        var uowMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<IPassageEventRepository>();
        uowMock.SetupGet(u => u.PassageEvents).Returns(repoMock.Object);
        uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new PassageEventService(uowMock.Object);

        // Act
        var result = await service.RegisterPassageAsync(decision, device, processedBy, AccessResult.Allowed, "ok");

        // Assert
        repoMock.Verify(r => r.AddAsync(It.IsAny<PassageEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(result.Result, Is.EqualTo(AccessResult.Allowed));
        Assert.That(result.Device.Id, Is.EqualTo(device.Id));
    }
}
