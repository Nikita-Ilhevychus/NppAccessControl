using NppAccessControl.DAL.Entities;
using NppAccessControl.DAL.Entities.Enums;

namespace NppAccessControl.BLL.Models;

public record AccessDecisionResult(
    AccessDecisionContext Context,
    AccessResult Result,
    string Reason,
    AccessPermission? GrantedPermission);
