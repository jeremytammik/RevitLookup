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

        return Directory.EnumerateFiles(userAddinsPath, "*.addin", SearchOption.AllDirectories)
            .Union(Directory.EnumerateFiles(machineAddinsPath, "*.addin", SearchOption.AllDirectories))
            .Union(Directory.EnumerateFiles(storeAddinsPath, "*.addin", SearchOption.AllDirectories))
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
                Console.WriteLine($"Bad file: {manifest}");
            }
        }

        return addinDirectories;
    }
}