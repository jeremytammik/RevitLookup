using ConsoleTables;
using DependenciesReport.Models;

namespace DependenciesReport;

public static class TableFormater
{
    public static ConsoleTable CreateReportTable(List<DirectoryDescriptor> dependenciesMap)
    {
        var conflictMap = GetConflictMap(dependenciesMap);

        var assemblyNames = conflictMap
            .SelectMany(dir => dir.Assemblies)
            .Select(assembly => assembly.Name)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        var table = new ConsoleTable(new[] { "Dependency" }.Concat(conflictMap.Select(dir => dir.Name)).ToArray());

        foreach (var assemblyName in assemblyNames)
        {
            var row = new List<object> { assemblyName };
            foreach (var dir in conflictMap)
            {
                var assembly = dir.Assemblies.FirstOrDefault(assembly => assembly.Name == assemblyName);
                row.Add(assembly?.Version.ToString(3) ?? "-");
            }

            table.AddRow(row.ToArray());
        }

        return table;
    }

    private static List<DirectoryDescriptor> GetConflictMap(List<DirectoryDescriptor> directories)
    {
        var versionMap = directories
            .SelectMany(directory => directory.Assemblies)
            .GroupBy(assembly => assembly.Name)
            .ToDictionary(
                group => group.Key, 
                group => group.Select(assembly => assembly.Version).Distinct().ToList());

        var uniqueAssemblies = new HashSet<AssemblyDescriptor>(
            directories
                .SelectMany(directory => directory.Assemblies)
                .Where(assembly => versionMap[assembly.Name].Count > 1));

        var conflictDirectories = new List<DirectoryDescriptor>();
        foreach (var directory in directories)
        {
            var conflictAssemblies = new List<AssemblyDescriptor>();
            foreach (var assembly in directory.Assemblies)
            {
                if (uniqueAssemblies.Contains(assembly)) conflictAssemblies.Add(assembly);
            }

            var conflictDirectory = new DirectoryDescriptor(directory.Name, directory.Path)
            {
                Assemblies = conflictAssemblies
            };
            
            if (conflictDirectory.Assemblies.Count > 0) conflictDirectories.Add(conflictDirectory);
        }

        return conflictDirectories;
    }
}