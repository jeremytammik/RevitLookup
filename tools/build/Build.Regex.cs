using System.Text.RegularExpressions;

sealed partial class Build
{
    readonly Regex ArgumentsRegex = ArgumentsRegexGenerator();

    [GeneratedRegex("'(.+?)'", RegexOptions.Compiled)]
    private static partial Regex ArgumentsRegexGenerator();
}