using Moq;
using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services;
using NppAccessControl.BLL.Services.Interfaces;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.BLL.Tests.Services;

public class TurnstileServiceTests
{
    [Test]
    public async Task ProcessCardReadAsync_CreatesIncident_WhenDenied()
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

        var device = new CardReader
        {
            Id = Guid.NewGuid(),
            SerialNumber = "SN",
            Model = "M",
            InstalledAt = DateTime.UtcNow,
            Checkpoint = checkpoint,
            InterfaceType = ReaderInterfaceType.Ethernet
        };

        var user = new UserAccount
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
            Device = device,
            ProcessedBy = user,
            EventTime = DateTime.UtcNow,
            Direction = PassageDirection.Entry,
            Result = AccessResult.Denied,
            EmergencyMode = EmergencyMode.Normal
        };

        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            Title = "Incident",
            Description = "desc",
            Severity = IncidentSeverity.Medium,
            Status = IncidentStatus.Open,
            CreatedAt = DateTime.UtcNow,
            ResolvedAt = DateTime.MinValue,
            SourceEvent = passageEvent,
            CreatedBy = user,
            ResolvedBy = user
        };

        var decisionService = new Mock<IAccessDecisionService>();
        var passageEventService = new Mock<IPassageEventService>();
        var incidentService = new Mock<IIncidentService>();

        decisionService.Setup(s => s.DecideAsync("C1", checkpoint.Id, system.Id, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(decision);

        passageEventService.Setup(s => s.RegisterPassageAsync(decision, device, user, AccessResult.Denied, decision.Reason, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passageEvent);

        incidentService.Setup(s => s.CreateIncidentForDeniedAccessAsync(decision, passageEvent, decision.Reason, user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(incident);

        var service = new TurnstileService(decisionService.Object, passageEventService.Object, incidentService.Object);

        // Act
        var result = await service.ProcessCardReadAsync("C1", checkpoint.Id, system.Id, DateTime.UtcNow, device, user);

        // Assert
        Assert.That(result.Result, Is.EqualTo(AccessResult.Denied));
        Assert.That(result.Incident, Is.Not.Null);
        incidentService.Verify(s => s.CreateIncidentForDeniedAccessAsync(decision, passageEvent, decision.Reason, user, It.IsAny<CancellationToken>()), Times.Once);
    }
}
