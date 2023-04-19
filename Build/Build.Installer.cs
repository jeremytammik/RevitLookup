using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Serilog;
using Serilog.Events;
using Logger = Serilog.Core.Logger;

partial class Build
{
    [GeneratedRegex("'(.+?)'", RegexOptions.Compiled)]
    private static partial Regex StreamRegexGenerator();

    readonly Regex StreamRegex = StreamRegexGenerator();

    Target CreateInstaller => _ => _
        .TriggeredBy(Compile)
        .OnlyWhenStatic(() => IsLocalBuild || GitRepository.IsOnMasterBranch())
        .Executes(() =>
        {
            var exeFilePattern = $"*{Solution.Installer.Name}.exe";
            var exeFile = Directory.EnumerateFiles(Solution.Installer.Directory, exeFilePattern, SearchOption.AllDirectories).First();

            var buildDirectories = Configurations
                .Select(config => $"* {config}*")
                .SelectMany(pattern => Directory.EnumerateDirectories(Solution.RevitLookup.Directory, pattern, SearchOption.AllDirectories))
                .ToList();

            if (buildDirectories.Count == 0)
                throw new Exception($"No files match the specified pattern to create an installer: {string.Join(",", Configurations)}");

            foreach (var buildDirectory in buildDirectories)
            {
                var proc = new Process();
                proc.StartInfo.FileName = exeFile;
                proc.StartInfo.Arguments = $@"""{buildDirectory}""";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                RedirectStream(proc.StandardOutput, LogEventLevel.Information);
                RedirectStream(proc.StandardError, LogEventLevel.Error);

                proc.WaitForExit();
                if (proc.ExitCode != 0) throw new Exception($"The installer creation failed with ExitCode {proc.ExitCode}");
            }
        });

    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void RedirectStream(StreamReader reader, LogEventLevel eventLevel)
    {
        while (!reader.EndOfStream)
        {
            var value = reader.ReadLine();
            if (value is null) continue;

            var matches = StreamRegex.Matches(value);
            if (matches.Count > 0)
            {
                var parameters = matches
                    .Select(match => match.Value.Substring(1, match.Value.Length - 2))
                    .Cast<object>()
                    .ToArray();

                var line = StreamRegex.Replace(value, match => $"{{Parameter{match.Index}}}");
                Log.Write(eventLevel, line, parameters);
            }
            else
            {
                Log.Debug(value);
            }
        }
    }
}