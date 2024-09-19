using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DependenciesReport.Utilities;

public static partial class AddinUtilities
{
    [GeneratedRegex(@"\d{4}")]
    private static partial Regex RevitVersionRegex();
    
    public static IGrouping<string, string>[] GetAddinLocations()
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var machineFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var builtinFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        var userAddinsPath = Path.Combine(userFolder, "Autodesk", "Revit", "Addins");
        var userStoreAddinsPath = Path.Combine(userFolder, "Autodesk", "ApplicationPlugins");
        
        var machineAddinsPath = Path.Combine(machineFolder, "Autodesk", "Revit", "Addins");
        var machineStoreAddinsPath = Path.Combine(machineFolder, "Autodesk", "ApplicationPlugins");
        
        var builtinAddinsPath = Path.Combine(builtinFolder, "Autodesk");

        var addinFiles = Enumerable.Empty<string>();

        if (Directory.Exists(userAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(userAddinsPath));
        if (Directory.Exists(userStoreAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(userStoreAddinsPath));
        if (Directory.Exists(machineAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(machineAddinsPath));
        if (Directory.Exists(machineStoreAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(machineStoreAddinsPath));
        if (Directory.Exists(builtinAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(builtinAddinsPath));

        return addinFiles
            .GroupBy(GroupByRevitVersion)
            .ToArray();
    }

    public static Dictionary<string, string> GetAddinDirectories(IGrouping<string, string> addinLocation)
    {
        var addinDirectories = new Dictionary<string, string>();

        foreach (var manifest in addinLocation)
        {
            try
            {
                var document = XDocument.Load(manifest);

                var addInElement = document.Descendants("AddIn").FirstOrDefault();
                if (addInElement == null) continue;

                var nameElement = addInElement.Element("Name");
                var name = nameElement != null ? nameElement.Value : Path.GetFileNameWithoutExtension(manifest);

                var assemblyElement = addInElement.Element("Assembly");
                if (assemblyElement == null) continue;

                var assemblyPath = assemblyElement.Value;
                if (!Path.IsPathRooted(assemblyPath))
                {
                    var directoryName = Path.GetDirectoryName(manifest)!;
                    assemblyPath = Path.Combine(directoryName, assemblyPath);
                }

                if (File.Exists(assemblyPath))
                {
                    addinDirectories[name] = Path.GetDirectoryName(assemblyPath)!;
                }
            }
            catch
            {
                Console.WriteLine($"Unsupported manifest: {manifest}. Skipped...");
            }
        }

        return addinDirectories;
    }

    private static IEnumerable<string> EnumerateAddins(string folder)
    {
        return Directory.EnumerateFiles(folder, "*.addin", SearchOption.AllDirectories)
            .Where(path => path.Contains("revit", StringComparison.OrdinalIgnoreCase));
    }

    private static string GroupByRevitVersion(string file)
    {
        var versionRegex = RevitVersionRegex();
        var matches = versionRegex.Matches(file);
        if (matches.Count == 0) return "Unknown Revit version";

        return matches[^1].Value;
    }
}