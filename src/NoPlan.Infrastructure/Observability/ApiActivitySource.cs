using System.Diagnostics;

namespace NoPlan.Infrastructure.Observability;

public static class ApiActivitySource
{
    public const string Name = nameof(ApiActivitySource);
    public static readonly ActivitySource Instance = new(Name);
}
