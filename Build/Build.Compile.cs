using System.IO.Enumeration;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target Compile => _ => _
        .TriggeredBy(Clean)
        .Executes(() =>
        {
            foreach (var configuration in GlobBuildConfigurations())
            {
                if (!VersionMap.TryGetValue(configuration, out var value)) value = "1.0.0";

                DotNetBuild(settings => settings
                    .SetConfiguration(configuration)
                    .SetVersion(value)
                    .SetVerbosity(DotNetVerbosity.Minimal));
            }
        });

    List<string> GlobBuildConfigurations()
    {
        var configurations = Solution.Configurations
            .Select(pair => pair.Key)
            .Select(config =>
            {
                var platformIndex = config.LastIndexOf('|');
                return config.Remove(platformIndex);
            })
            .Where(config =>
            {
                foreach (var wildcard in Configurations)
                    if (FileSystemName.MatchesSimpleExpression(wildcard, config))
                        return true;

                return false;
            })
            .ToList();

        if (configurations.Count == 0)
            throw new Exception($"No solution configurations have been found. Pattern: {string.Join(" | ", Configurations)}");

        return configurations;
    }
}