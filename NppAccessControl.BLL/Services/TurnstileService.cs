using NppAccessControl.BLL.Models;
using NppAccessControl.BLL.Services.Interfaces;
using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.BLL.Services;

public class TurnstileService(
    IAccessDecisionService accessDecisionService,
    IPassageEventService passageEventService,
    IIncidentService incidentService) : ITurnstileService
{
    public async Task<TurnstileProcessResult> ProcessCardReadAsync(
        string cardNumber,
        Guid checkpointId,
        Guid systemId,
        DateTime readTime,
        Device device,
        UserAccount processedBy,
        CancellationToken cancellationToken = default)
    {
        var decision = await accessDecisionService.DecideAsync(cardNumber, checkpointId, systemId, readTime, cancellationToken);

        if (decision.Context.Card is null || decision.Context.Checkpoint is null || decision.Context.System is null)
        {
            return new TurnstileProcessResult(decision.Result, decision.Reason, null, null);
        }

        var eventResult = await passageEventService.RegisterPassageAsync(
            decision,
            device,
            processedBy,
            decision.Result,
            decision.Reason,
            cancellationToken);

        Incident? incident = null;
        if (decision.Result == AccessResult.Denied)
        {
            incident = await incidentService.CreateIncidentForDeniedAccessAsync(decision, eventResult, decision.Reason, processedBy, cancellationToken);
        }

        return new TurnstileProcessResult(decision.Result, decision.Reason, eventResult, incident);
    }
}
