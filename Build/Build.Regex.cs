using System.Text.RegularExpressions;

sealed partial class Build
{
    readonly Regex StreamRegex = StreamRegexGenerator();

    [GeneratedRegex("'(.+?)'", RegexOptions.Compiled)]
    private static partial Regex StreamRegexGenerator();
}