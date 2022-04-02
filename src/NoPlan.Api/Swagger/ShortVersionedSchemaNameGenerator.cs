using System.Text.RegularExpressions;
using NJsonSchema.Generation;

namespace NoPlan.Api.Swagger;

public sealed class ShortVersionedSchemaNameGenerator : ISchemaNameGenerator
{
    private static readonly Regex VersionPattern = new(@"^.*\.([V]\d)\..*$", RegexOptions.Compiled);

    public string Generate(Type type)
    {
        var matcher = VersionPattern.Match(type.Namespace!);
        return matcher.Success
            ? $"{matcher.Groups[1].Value}-{type.Name}"
            : type.FullName!;
    }
}
