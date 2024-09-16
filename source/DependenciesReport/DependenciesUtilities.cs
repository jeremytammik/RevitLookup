using System.Diagnostics;
using DependenciesReport.Models;

namespace DependenciesReport;

public static class DependenciesTools
{
    public static List<DirectoryDescriptor> CreateDependenciesMap(Dictionary<string, string> addinDirectories)
    {
        var dependenciesMap = new List<DirectoryDescriptor>();

        foreach (var directoryMetadata in addinDirectories)
        {
            var directoryDescriptor = new DirectoryDescriptor(directoryMetadata.Key, directoryMetadata.Value);
            var assemblies = Directory.GetFiles(directoryMetadata.Value, "*.dll");

            foreach (var assembly in assemblies)
            {
                try
                {
                    var assemblyName = Path.GetFileName(assembly);
                    var fileInfo = FileVersionInfo.GetVersionInfo(assembly);
                    var assemblyVersion = new Version(fileInfo.FileMajorPart, fileInfo.FileMinorPart, fileInfo.FileBuildPart, fileInfo.FilePrivatePart);

                    var assemblyDescriptor = new AssemblyDescriptor(assemblyName, assembly, assemblyVersion);
                    directoryDescriptor.Assemblies.Add(assemblyDescriptor);
                }
                catch
                {
                    Console.WriteLine($"Assembly without metadata: {assembly}. Skipped...");
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