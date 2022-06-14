// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.Options;

public interface IOptionsSectionDefinition
{
    /// <summary>
    ///     The <see cref="IConfiguration" /> section name.
    /// </summary>
    static abstract string SectionName { get; }
}
