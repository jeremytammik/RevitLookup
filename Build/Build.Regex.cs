using System.Text.RegularExpressions;

partial class Build
{
    [GeneratedRegex("'(.+?)'", RegexOptions.Compiled)]
    private static partial Regex StreamRegexGenerator();

    [GeneratedRegex(@"(\d+\.)+\d+", RegexOptions.Compiled)]
    private static partial Regex VersionRegexGenerator();
    
    [GeneratedRegex(@"\d{4}")]
    private static partial Regex YearRegexGenerator();
    
    readonly Regex StreamRegex = StreamRegexGenerator();
    readonly Regex VersionRegex = VersionRegexGenerator();
    readonly Regex YearRegex = YearRegexGenerator();
}