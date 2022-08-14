using Pulumi.AzureNative.Authorization;

namespace NoPlan.Infrastructure;

public sealed class RoleAssignments
{
    public List<RoleAssignment> Assignments { get; }

    public RoleAssignments(IEnumerable<RoleAssignmentMapping> roleAssignments, RoleDefinitions roleMappings) =>
        Assignments = roleAssignments.Select(r =>
            new RoleAssignment($"{r.Principal.Name}-{r.Resource.GetResourceName()}-{r.RoleName}",
                new()
                {
                    PrincipalId = r.Principal.Id,
                    PrincipalType = r.Principal.Type,
                    RoleAssignmentName = r.Name,
                    RoleDefinitionId = r.RoleDefinitionId(roleMappings),
                    Scope = r.Resource.Id
                })).ToList();
}
