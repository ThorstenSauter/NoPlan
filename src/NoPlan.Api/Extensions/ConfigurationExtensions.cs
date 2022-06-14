using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSectionedOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, IOptionsSectionDefinition
    {
        ArgumentNullException.ThrowIfNull(services);
        return services.Configure<TOptions>(configuration.GetSection(TOptions.SectionName));
    }
}
