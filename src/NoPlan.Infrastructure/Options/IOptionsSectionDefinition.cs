// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.Options;

/// <summary>
///     Allows for generic registration of <see cref="IOptions{TOptions}" /> by providing a static name of
///     <see cref="IConfiguration" /> section.
/// </summary>
public interface IOptionsSectionDefinition
{
    /// <summary>
    ///     Gets the <see cref="IConfiguration" /> section name.
    /// </summary>
    static abstract string SectionName { get; }
}
