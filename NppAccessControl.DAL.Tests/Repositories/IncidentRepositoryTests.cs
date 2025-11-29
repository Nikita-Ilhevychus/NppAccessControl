using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Tests.Repositories.Support;

namespace NppAccessControl.DAL.Tests.Repositories;

public class IncidentRepositoryTests
{
    [Test]
    public async Task GetOpenByCardAsync_ReturnsOnlyOpenOrInProgressForCard()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetOpenByCardAsync_ReturnsOnlyOpenOrInProgressForCard));
        var cardId = Guid.NewGuid();

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            BirthDate = new DateTime(1990, 1, 1),
            Phone = "123",
            Department = "IT",
            PersonnelNumber = "1",
            Position = "Dev"
        };

        var card = new AccessCard
        {
            Id = cardId,
            CardNumber = "C1",
            Type = CardType.Employee,
            Status = CardStatus.Active,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            ExpiryDate = DateTime.UtcNow.AddDays(5),
            Owner = employee,
            Profile = new AccessProfile { Id = Guid.NewGuid(), Name = "Default" }
        };

        var checkpoint = new Checkpoint
        {
            Id = Guid.NewGuid(),
            Code = "CP1",
            Name = "Gate",
            Location = "A",
            IsActive = true
        };

        var zone = new AccessZone
        {
            Id = Guid.NewGuid(),
            Code = "Z1",
            Name = "Zone",
            SecurityLevel = SecurityLevel.Normal
        };

        var device = new CardReader
        {
            Id = Guid.NewGuid(),
            SerialNumber = "SN",
            Model = "M",
            InstalledAt = DateTime.UtcNow.AddDays(-3),
            Checkpoint = checkpoint,
            InterfaceType = ReaderInterfaceType.Wiegand
        };

        var processor = new UserAccount
        {
            Id = Guid.NewGuid(),
            Login = "processor",
            PasswordHash = "hash",
            IsActive = true,
            Owner = employee,
            Roles = []
        };

        var sourceEvent = new PassageEvent
        {
            Id = Guid.NewGuid(),
            Card = card,
            Person = employee,
            Checkpoint = checkpoint,
            Zone = zone,
            Device = device,
            ProcessedBy = processor,
            EventTime = DateTime.UtcNow,
            Direction = PassageDirection.Entry,
            Result = AccessResult.Denied,
            EmergencyMode = EmergencyMode.Normal
        };

        var openIncident = new Incident
        {
            Id = Guid.NewGuid(),
            Title = "Open",
            Description = "desc",
            Severity = IncidentSeverity.Medium,
            Status = IncidentStatus.Open,
            CreatedAt = DateTime.UtcNow,
            ResolvedAt = DateTime.MinValue,
            SourceEvent = sourceEvent,
            CreatedBy = processor,
            ResolvedBy = processor
        };

        var inProgressIncident = openIncident with
        {
            Id = Guid.NewGuid(),
            Title = "In progress",
            Status = IncidentStatus.InProgress
        };

        var closedIncidentForCard = openIncident with
        {
            Id = Guid.NewGuid(),
            Title = "Closed",
            Status = IncidentStatus.Closed
        };

        var incidentOtherCard = openIncident with
        {
            Id = Guid.NewGuid(),
            Title = "Other card",
            SourceEvent = sourceEvent with { Id = Guid.NewGuid(), Card = card with { Id = Guid.NewGuid(), CardNumber = "C2" } }
        };

        await context.AddRangeAsync(card, checkpoint, zone, device, processor, sourceEvent, openIncident, inProgressIncident, closedIncidentForCard, incidentOtherCard);
        await context.SaveChangesAsync();

        var repository = new IncidentRepository(context);

        // Act
        var result = await repository.GetOpenByCardAsync(cardId);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Status), Is.EquivalentTo(new[] { IncidentStatus.Open, IncidentStatus.InProgress }));
    }
}
