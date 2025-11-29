using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services.Interfaces;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;
using NppAccessControl.DAL.Repositories.Interfaces;
using NppAccessControl.DAL.UnitOfWork.Interfaces;

namespace NppAccessControl.BLL.Services;

public class PassageEventService(IUnitOfWork unitOfWork) : IPassageEventService
{
    public async Task<PassageEvent> RegisterPassageAsync(
        AccessDecisionResult decision,
        Device device,
        UserAccount processedBy,
        AccessResult resultOverride,
        string reason,
        CancellationToken cancellationToken = default)
    {
        if (decision.Context.Card is null || decision.Context.Checkpoint is null || decision.Context.System is null)
        {
            throw new InvalidOperationException("Decision context is incomplete for passage registration.");
        }

        var card = decision.Context.Card;
        var checkpoint = decision.Context.Checkpoint;
        var zone = decision.GrantedPermission?.Zone ?? checkpoint.Zones.FirstOrDefault();
        if (zone is null)
        {
            throw new InvalidOperationException("Zone is required to register passage.");
        }

        var passageEvent = new PassageEvent
        {
            Id = Guid.NewGuid(),
            EventTime = decision.Context.ReadTime,
            Direction = PassageDirection.Entry,
            Result = resultOverride,
            Reason = reason,
            EmergencyMode = decision.Context.System.EmergencyMode,
            Card = card,
            Person = card.Owner,
            Checkpoint = checkpoint,
            Zone = zone,
            Device = device,
            ProcessedBy = processedBy
        };

        await unitOfWork.PassageEvents.AddAsync(passageEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return passageEvent;
    }
}
