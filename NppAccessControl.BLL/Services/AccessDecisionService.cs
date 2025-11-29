using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services.Interfaces;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Services;

public class AccessDecisionService(IUnitOfWork unitOfWork) : IAccessDecisionService
{
    public async Task<AccessDecisionResult> DecideAsync(
        string cardNumber,
        Guid checkpointId,
        Guid systemId,
        DateTime readTime,
        CancellationToken cancellationToken = default)
    {
        var card = await unitOfWork.AccessCards.GetByCardNumberWithDetailsAsync(cardNumber, cancellationToken);
        if (card is null)
        {
            return Denied("Card not found", readTime);
        }

        var checkpoint = await unitOfWork.Checkpoints.GetByIdWithZonesAndDevicesAsync(checkpointId, cancellationToken);
        if (checkpoint is null)
        {
            return Denied("Checkpoint not found", readTime, card);
        }

        var system = await unitOfWork.AccessControlSystems.GetByIdWithTopologyAsync(systemId, cancellationToken);
        if (system is null)
        {
            return Denied("Access control system not found", readTime, card, checkpoint);
        }

        if (card.Status != CardStatus.Active)
        {
            return Denied("Card is not active", readTime, card, checkpoint, system);
        }

        if (card.ExpiryDate < readTime || card.IssueDate > readTime)
        {
            return Denied("Card is outside validity dates", readTime, card, checkpoint, system);
        }

        if (system.EmergencyMode == EmergencyMode.Emergency && checkpoint.Zones.Any(z => z.SecurityLevel is SecurityLevel.High or SecurityLevel.Critical))
        {
            return Denied("Emergency mode blocks high security zones", readTime, card, checkpoint, system);
        }

        var permittedZoneIds = checkpoint.Zones.Select(z => z.Id).ToHashSet();
        var matchedPermission = card.Profile.Permissions.FirstOrDefault(p =>
            permittedZoneIds.Contains(p.Zone.Id) &&
            p.ValidFrom <= readTime &&
            p.ValidTo >= readTime &&
            IsWithinDailyWindow(p, readTime));

        if (matchedPermission is null)
        {
            return Denied("No valid permission for checkpoint", readTime, card, checkpoint, system);
        }

        return new AccessDecisionResult(
            new AccessDecisionContext(card, checkpoint, system, readTime),
            AccessResult.Allowed,
            "Access granted",
            matchedPermission);
    }

    private static bool IsWithinDailyWindow(AccessPermission permission, DateTime readTime)
    {
        var time = TimeOnly.FromDateTime(readTime);
        return time >= permission.TimeFrom && time <= permission.TimeTo;
    }

    private AccessDecisionResult Denied(
        string reason,
        DateTime readTime,
        AccessCard? card = null,
        Checkpoint? checkpoint = null,
        AccessControlSystem? system = null) =>
        new(
            new AccessDecisionContext(card, checkpoint, system, readTime),
            AccessResult.Denied,
            reason,
            null);
}
