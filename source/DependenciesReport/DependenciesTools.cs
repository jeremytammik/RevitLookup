namespace DependenciesReport;

public static class DependenciesTools
{
    public static Dictionary<string, Dictionary<string, string>> CreateDependenciesMap(string[] directories)
    {
        var dependenciesMap = new Dictionary<string, Dictionary<string, string>>();
        foreach (var directory in directories)
        {
            var assemblies = Directory.GetFiles(directory, "*.dll");

            foreach (var assembly in assemblies)
            {
                var assemblyName = Path.GetFileName(assembly);
                var assemblyVersion = AssemblyUtils.GetAssemblyVersion(assembly);

                if (!dependenciesMap.TryGetValue(assemblyName, out var value))
                {
                    value = new Dictionary<string, string>();
                    dependenciesMap[assemblyName] = value;
                }

                if (assemblyVersion != null)
                {
                    value[Path.GetFileName(directory)] = assemblyVersion;
                }
            }
        }

        return dependenciesMap;
    }

    public static void UpgradeDependencies(Dictionary<string, Dictionary<string, Dictionary<string, string>>> dependenciesMaps)
    {
        foreach (var (yearDirectory, dependenciesMap) in dependenciesMaps)
        {
            foreach (var assemblyName in dependenciesMap.Keys)
            {
                string? maxVersion = null;
                string? maxVersionDirectory = null;

                foreach (var directory in dependenciesMap[assemblyName].Keys)
                {
                    var version = dependenciesMap[assemblyName][directory];
                    if (maxVersion == null || CompareVersions(version, maxVersion) > 0)
                    {
                        maxVersion = version;
                        maxVersionDirectory = directory;
                    }
                }
                
                if (maxVersionDirectory is not null && maxVersion is not null)
                {
                    var maxVersionFilePath = Path.Combine(yearDirectory, maxVersionDirectory, assemblyName);

                    foreach (var directory in dependenciesMap[assemblyName].Keys)
                    {
                        if (directory != maxVersionDirectory)
                        {
                            var targetFilePath = Path.Combine(yearDirectory, directory, assemblyName);
                            var targetVersion = dependenciesMap[assemblyName][directory];

                            if (CompareVersions(targetVersion, maxVersion) < 0)
                            {
                                File.Copy(maxVersionFilePath, targetFilePath, true);
                                Console.WriteLine($"Assembly {targetFilePath} was upgraded from version {targetVersion} to version {maxVersion}.");
                            }
                        }
                    }
                }
            }
        }
    }

    private static int CompareVersions(string version1, string version2)
    {
        var originVersion = new Version(version1);
        var targetVersion = new Version(version2);
        return originVersion.CompareTo(targetVersion);
    }
}