using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Static class containing extension methods for the <see cref="IServiceCollection" /> type.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Registers <see cref="IOptions{TOptions}" /> from a section of <see cref="IConfiguration" /> specified by the
    ///     <see cref="IOptionsSectionDefinition" />.
    /// </summary>
    /// <param name="services">The service collection to add the <see cref="IOptions{TOptions}" /> to.</param>
    /// <param name="configuration">
    ///     The configuration object containing the section to use for
    ///     <see cref="IOptions{TOptions}" /> registration.
    /// </param>
    /// <typeparam name="TOptions">The options type to register.</typeparam>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddSectionedOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, IOptionsSectionDefinition
    {
        ArgumentNullException.ThrowIfNull(services);
        return services.Configure<TOptions>(configuration.GetSection(TOptions.SectionName));
    }
}
