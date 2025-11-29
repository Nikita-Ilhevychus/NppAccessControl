using NppAccessControl.DAL.Entities;

namespace NppAccessControl.BLL.Models;

public record AccessDecisionContext(
    AccessCard? Card,
    Checkpoint? Checkpoint,
    AccessControlSystem? System,
    DateTime ReadTime);
