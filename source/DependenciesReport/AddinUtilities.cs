using System.Xml.Linq;

namespace DependenciesReport;

public static class AddinUtilities
{
    public static IGrouping<string, string>[] GetAddinLocations()
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var machineFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        var userAddinsPath = Path.Combine(userFolder, "Autodesk", "Revit", "Addins");
        var machineAddinsPath = Path.Combine(machineFolder, "Autodesk", "Revit", "Addins");
        var storeAddinsPath = Path.Combine(machineFolder, "Autodesk", "ApplicationPlugins");

        var addinFiles = Enumerable.Empty<string>();

        if (Directory.Exists(userAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(userAddinsPath));
        if (Directory.Exists(machineAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(machineAddinsPath));
        if (Directory.Exists(storeAddinsPath)) addinFiles = addinFiles.Union(EnumerateAddins(storeAddinsPath));

        return addinFiles
            .GroupBy(file => Path.GetFileName(Path.GetDirectoryName(file))!)
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
        return Directory.EnumerateFiles(folder, "*.addin", SearchOption.AllDirectories);
    }
}