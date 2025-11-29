using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.BLL.Models;

public record TurnstileProcessResult(
    AccessResult Result,
    string Reason,
    PassageEvent? PassageEvent,
    Incident? Incident);
