using Moq;
using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Tests.Services;

public class IncidentServiceTests
{
    [Test]
    public async Task CreateIncidentForDeniedAccessAsync_CreatesIncident_WhenDenied()
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

        var checkpoint = new Checkpoint { Id = Guid.NewGuid(), Code = "CP1", Name = "Gate", Location = "A", IsActive = true };
        var system = new AccessControlSystem { Id = Guid.NewGuid(), Name = "Sys", EmergencyMode = EmergencyMode.Normal };

        var context = new AccessDecisionContext(card, checkpoint, system, DateTime.UtcNow);
        var decision = new AccessDecisionResult(context, AccessResult.Denied, "denied", null);

        var processor = new UserAccount
        {
            Id = Guid.NewGuid(),
            Login = "processor",
            PasswordHash = "hash",
            IsActive = true,
            Owner = employee,
            Roles = []
        };

        var passageEvent = new PassageEvent
        {
            Id = Guid.NewGuid(),
            Card = card,
            Person = card.Owner,
            Checkpoint = checkpoint,
            Zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z1", Name = "Zone", SecurityLevel = SecurityLevel.Normal },
            Device = new CardReader
            {
                Id = Guid.NewGuid(),
                SerialNumber = "SN",
                Model = "M",
                InstalledAt = DateTime.UtcNow,
                Checkpoint = checkpoint,
                InterfaceType = ReaderInterfaceType.Ethernet
            },
            ProcessedBy = processor,
            EventTime = DateTime.UtcNow,
            Direction = PassageDirection.Entry,
            Result = AccessResult.Denied,
            EmergencyMode = EmergencyMode.Normal
        };

        var uowMock = new Mock<IUnitOfWork>();
        var incidentRepo = new Mock<IIncidentRepository>();
        uowMock.SetupGet(u => u.Incidents).Returns(incidentRepo.Object);
        uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new IncidentService(uowMock.Object);

        // Act
        var incident = await service.CreateIncidentForDeniedAccessAsync(decision, passageEvent, "reason", processor);

        // Assert
        Assert.That(incident, Is.Not.Null);
        incidentRepo.Verify(r => r.AddAsync(It.IsAny<Incident>(), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateIncidentForDeniedAccessAsync_ReturnsNull_WhenAllowed()
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

        var checkpoint = new Checkpoint { Id = Guid.NewGuid(), Code = "CP1", Name = "Gate", Location = "A", IsActive = true };
        var system = new AccessControlSystem { Id = Guid.NewGuid(), Name = "Sys", EmergencyMode = EmergencyMode.Normal };

        var context = new AccessDecisionContext(card, checkpoint, system, DateTime.UtcNow);
        var decision = new AccessDecisionResult(context, AccessResult.Allowed, "ok", null);

        var passageEvent = new PassageEvent
        {
            Id = Guid.NewGuid(),
            Card = card,
            Person = card.Owner,
            Checkpoint = checkpoint,
            Zone = new AccessZone { Id = Guid.NewGuid(), Code = "Z1", Name = "Zone", SecurityLevel = SecurityLevel.Normal },
            Device = new CardReader
            {
                Id = Guid.NewGuid(),
                SerialNumber = "SN",
                Model = "M",
                InstalledAt = DateTime.UtcNow,
                Checkpoint = checkpoint,
                InterfaceType = ReaderInterfaceType.Ethernet
            },
            ProcessedBy = new UserAccount
            {
                Id = Guid.NewGuid(),
                Login = "processor",
                PasswordHash = "hash",
                IsActive = true,
                Owner = employee,
                Roles = []
            },
            EventTime = DateTime.UtcNow,
            Direction = PassageDirection.Entry,
            Result = AccessResult.Allowed,
            EmergencyMode = EmergencyMode.Normal
        };

        var uowMock = new Mock<IUnitOfWork>();
        var incidentRepo = new Mock<IIncidentRepository>();
        uowMock.SetupGet(u => u.Incidents).Returns(incidentRepo.Object);

        var service = new IncidentService(uowMock.Object);

        // Act
        var incident = await service.CreateIncidentForDeniedAccessAsync(decision, passageEvent, "reason", passageEvent.ProcessedBy);

        // Assert
        Assert.That(incident, Is.Null);
        incidentRepo.Verify(r => r.AddAsync(It.IsAny<Incident>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
