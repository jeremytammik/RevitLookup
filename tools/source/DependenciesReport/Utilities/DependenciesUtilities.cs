using System.Diagnostics;
using DependenciesReport.Models;

namespace DependenciesReport.Utilities;

public static class DependenciesTools
{
    public static List<DirectoryDescriptor> CreateDependenciesMap(string version, Dictionary<string, string> addinDirectories)
    {
        var dependenciesMap = new List<DirectoryDescriptor>();

        foreach (var directoryMetadata in addinDirectories)
        {
            var directoryDescriptor = new DirectoryDescriptor(directoryMetadata.Key, directoryMetadata.Value, version);
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
                        if (!ValidateIsolationContext(entry.Directory, entry.Assembly)) continue;

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

    private static bool ValidateIsolationContext(DirectoryDescriptor directory, AssemblyDescriptor assembly)
    {
        if (!int.TryParse(directory.Version, out var domainVersion)) return true;
        if (domainVersion < 2025) return true;

        const string isolationProvider = "Nice3point.Revit.Toolkit.dll";
        if (directory.Assemblies.Any(descriptor => descriptor.Name == isolationProvider))
        {
            if (assembly.Name == isolationProvider) return true;
            return false;
        }

        return true;
    }
}