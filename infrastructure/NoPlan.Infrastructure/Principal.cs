using Pulumi;
using Pulumi.AzureNative.Authorization;

namespace NoPlan.Infrastructure;

public sealed record Principal(string Name, Input<string> Id, PrincipalType Type);
