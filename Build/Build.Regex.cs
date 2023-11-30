using System.Text.RegularExpressions;

sealed partial class Build
{
    readonly Regex StreamRegex = StreamRegexGenerator();
    readonly Regex VersionRegex = VersionRegexGenerator();

    [GeneratedRegex("'(.+?)'", RegexOptions.Compiled)]
    private static partial Regex StreamRegexGenerator();

    [GeneratedRegex(@"(\d+\.)+\d+", RegexOptions.Compiled)]
    private static partial Regex VersionRegexGenerator();
}