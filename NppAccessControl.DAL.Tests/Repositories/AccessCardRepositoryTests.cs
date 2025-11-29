using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories;
using NppAccessControl.DAL.Tests.Repositories.Support;

namespace NppAccessControl.DAL.Tests.Repositories;

public class AccessCardRepositoryTests
{
    [Test]
    public async Task GetByCardNumberWithDetailsAsync_ReturnsCardWithOwnerAndPermissions()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByCardNumberWithDetailsAsync_ReturnsCardWithOwnerAndPermissions));

        var owner = new Employee
        {
            Id = Guid.NewGuid(),
            FullName = "Boris Johnson",
            BirthDate = new DateTime(1990, 1, 1),
            Phone = "123",
            Department = "IT",
            PersonnelNumber = "PN-1",
            Position = "Engineer"
        };

        var profile = new AccessProfile
        {
            Id = Guid.NewGuid(),
            Name = "Standard",
            Permissions = []
        };

        var zone = new AccessZone
        {
            Id = Guid.NewGuid(),
            Code = "Z1",
            Name = "Zone 1",
            SecurityLevel = SecurityLevel.Normal
        };

        var permission = new AccessPermission
        {
            Id = Guid.NewGuid(),
            Profile = profile,
            Zone = zone,
            ValidFrom = DateTime.UtcNow.AddDays(-1),
            ValidTo = DateTime.UtcNow.AddDays(1),
            TimeFrom = new TimeOnly(8, 0),
            TimeTo = new TimeOnly(18, 0)
        };

        profile.Permissions.Add(permission);

        var card = new AccessCard
        {
            Id = Guid.NewGuid(),
            CardNumber = "123",
            Type = CardType.Employee,
            Status = CardStatus.Active,
            IssueDate = DateTime.UtcNow.AddDays(-10),
            ExpiryDate = DateTime.UtcNow.AddDays(10),
            Owner = owner,
            Profile = profile
        };

        await context.AddRangeAsync(owner, zone, profile, permission, card);
        await context.SaveChangesAsync();

        var repository = new AccessCardRepository(context);

        // Act
        var result = await repository.GetByCardNumberWithDetailsAsync("123");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Owner.FullName, Is.EqualTo(owner.FullName));
        Assert.That(result.Profile.Permissions, Has.Count.EqualTo(1));
        Assert.That(result.Profile.Permissions[0].Zone.Code, Is.EqualTo(zone.Code));
    }

    [Test]
    public async Task GetByCardNumberWithDetailsAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = TestDbContextFactory.Create(nameof(GetByCardNumberWithDetailsAsync_ReturnsNull_WhenNotFound));
        var repository = new AccessCardRepository(context);

        // Act
        var result = await repository.GetByCardNumberWithDetailsAsync("missing");

        // Assert
        Assert.That(result, Is.Null);
    }
}
