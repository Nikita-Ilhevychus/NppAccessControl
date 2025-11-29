using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Tests.Repositories.Support;

namespace NppAccessControl.DAL.Tests.Repositories;

public class PassageEventRepositoryTests
{
    [Test]
    public async Task GetForCardAsync_FiltersByCardAndDateRange()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetForCardAsync_FiltersByCardAndDateRange));
        var cardId = Guid.NewGuid();

        var profile = new AccessProfile
        {
            Id = Guid.NewGuid(),
            Name = "Standard"
        };

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FullName = "Employee",
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
            IssueDate = DateTime.UtcNow.AddDays(-30),
            ExpiryDate = DateTime.UtcNow.AddDays(30),
            Owner = employee,
            Profile = profile
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

        var inRangeEvent = new PassageEvent
        {
            Id = Guid.NewGuid(),
            Card = card,
            Person = employee,
            Checkpoint = checkpoint,
            Zone = zone,
            Device = new CardReader
            {
                Id = Guid.NewGuid(),
                SerialNumber = "SN",
                Model = "M",
                InstalledAt = DateTime.UtcNow.AddDays(-20),
                Checkpoint = checkpoint,
                InterfaceType = ReaderInterfaceType.Ethernet
            },
            ProcessedBy = new UserAccount
            {
                Id = Guid.NewGuid(),
                Login = "admin",
                PasswordHash = "hash",
                IsActive = true,
                Owner = employee,
                Roles = []
            },
            EventTime = DateTime.UtcNow.AddHours(-1),
            Direction = PassageDirection.Entry,
            Result = AccessResult.Allowed,
            EmergencyMode = EmergencyMode.Normal
        };

        var outOfRangeEvent = inRangeEvent with
        {
            Id = Guid.NewGuid(),
            EventTime = DateTime.UtcNow.AddDays(-10)
        };

        var otherCardEvent = inRangeEvent with
        {
            Id = Guid.NewGuid(),
            Card = card with { Id = Guid.NewGuid(), CardNumber = "C2" },
            EventTime = DateTime.UtcNow.AddMinutes(-30)
        };

        await context.AddRangeAsync(profile, card, checkpoint, zone, inRangeEvent.Device, inRangeEvent.ProcessedBy, inRangeEvent.Person, inRangeEvent, outOfRangeEvent, otherCardEvent);
        await context.SaveChangesAsync();

        var repository = new PassageEventRepository(context);
        var from = DateTime.UtcNow.AddHours(-2);
        var to = DateTime.UtcNow;

        // Act
        var result = await repository.GetForCardAsync(cardId, from, to);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Id, Is.EqualTo(inRangeEvent.Id));
        Assert.That(result[0].Checkpoint.Id, Is.EqualTo(checkpoint.Id));
    }

    [Test]
    public async Task GetForCardAsync_ReturnsEmpty_WhenNothingMatches()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetForCardAsync_ReturnsEmpty_WhenNothingMatches));
        var repository = new PassageEventRepository(context);
        var from = DateTime.UtcNow.AddHours(-1);
        var to = DateTime.UtcNow;

        // Act
        var result = await repository.GetForCardAsync(Guid.NewGuid(), from, to);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
