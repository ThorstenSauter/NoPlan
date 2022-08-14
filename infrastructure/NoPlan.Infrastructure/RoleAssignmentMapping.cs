using Pulumi;

namespace NoPlan.Infrastructure;

public sealed record RoleAssignmentMapping(string Name, Principal Principal, string RoleName, CustomResource Resource)
{
    public string RoleDefinitionId(RoleDefinitions definitions) =>
        definitions[RoleName];
}
