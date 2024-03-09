using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using RevitLookup.Utils;

namespace RevitLookup.Config;

public static class FoldersConfiguration
{
    public static void AddFoldersConfiguration(this IConfigurationManager configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var writeAccess = AccessUtils.CheckWriteAccess(assemblyLocation) && !assemblyLocation.StartsWith(appDataPath);

        var targetFrameworkAttributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
        var targetFrameworkAttribute = (TargetFrameworkAttribute) targetFrameworkAttributes.First();

        var rootPath = configuration.GetValue<string>("contentRoot");

        configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"Assembly", assemblyLocation},
            {"Framework", targetFrameworkAttribute.FrameworkDisplayName},
            {"AddinVersion", new Version(versionInfo.FileVersion!).ToString(3)},
            {"FolderAccess", writeAccess ? "Write" : "Read"}
        });

        configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"ConfigFolder", Path.Combine(rootPath, "Config")},
            {"DownloadFolder", Path.Combine(rootPath, "Downloads")},
        });

        configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"SettingsPath", Path.Combine(configuration.GetValue<string>("ConfigFolder"), "Settings.cfg")},
        });
    }
}