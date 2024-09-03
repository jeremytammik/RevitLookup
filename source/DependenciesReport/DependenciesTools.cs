using System.Reflection;
using DependenciesReport.Models;

namespace DependenciesReport;

public static class DependenciesTools
{
    public static List<DirectoryDescriptor> CreateDependenciesMap(string[] directories)
    {
        var dependenciesMap = new List<DirectoryDescriptor>();

        foreach (var directory in directories)
        {
            var directoryDescriptor = new DirectoryDescriptor(Path.GetFileName(directory), directory);
            var assemblies = Directory.GetFiles(directory, "*.dll");

            foreach (var assembly in assemblies)
            {
                try
                {
                    var assemblyName = Path.GetFileName(assembly);
                    var assemblyVersion = AssemblyName.GetAssemblyName(assembly).Version ?? new Version();

                    var assemblyDescriptor = new AssemblyDescriptor(assemblyName, assembly, assemblyVersion);
                    directoryDescriptor.Assemblies.Add(assemblyDescriptor);
                }
                catch
                {
                    // ignored
                }
            }

            dependenciesMap.Add(directoryDescriptor);
        }

        return dependenciesMap;
    }

    public static void UpgradeDependencies(List<List<DirectoryDescriptor>> dependenciesMaps)
    {
        foreach (var directories in dependenciesMaps)
        {
            var assemblyGroups = directories
                .SelectMany(directory => directory.Assemblies, (dir, assembly) => new { Directory = dir, Assembly = assembly })
                .GroupBy(x => x.Assembly.Name);

            foreach (var assemblyGroup in assemblyGroups)
            {
                var maxAssembly = assemblyGroup.MaxBy(x => x.Assembly.Version);
                if (maxAssembly is null) continue;

                var maxVersion = maxAssembly.Assembly.Version;
                var maxVersionFilePath = maxAssembly.Assembly.Path;

                foreach (var entry in assemblyGroup)
                {
                    if (entry.Assembly.Version.CompareTo(maxVersion) < 0)
                    {
                        try
                        {
                            File.Copy(maxVersionFilePath, entry.Assembly.Path, true);
                            Console.WriteLine($"Assembly {entry.Assembly.Path} was upgraded from version {entry.Assembly.Version.ToString(3)} to {maxVersion.ToString(3)}.");
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine($"No access to upgrade {entry.Assembly.Path}");
                        }
                    }
                }
            }
        }
    }
}