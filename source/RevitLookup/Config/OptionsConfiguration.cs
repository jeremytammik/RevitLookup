using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Models.Options;
using RevitLookup.Utils;

namespace RevitLookup.Config;

public static class OptionsConfiguration
{
    public static void AddOptions(this ConfigurationManager configuration, IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        
        var targetFrameworkAttributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
        var targetFrameworkAttribute = (TargetFrameworkAttribute) targetFrameworkAttributes.First();
        
        var rootPath = configuration.GetValue<string>("contentRoot");
        var fileVersion = new Version(versionInfo.FileVersion!);

        services.Configure<AssemblyInfo>(info =>
        {
            info.Framework = targetFrameworkAttribute.FrameworkDisplayName;
            info.AddinVersion = new Version(fileVersion.Major, fileVersion.Minor, fileVersion.Build);
            info.IsAdminInstallation = assemblyLocation.StartsWith(appDataPath) || !AccessUtils.CheckWriteAccess(assemblyLocation);
        });

        services.Configure<FolderLocations>(info =>
        {
            info.RootFolder = rootPath;
            info.ConfigFolder = Path.Combine(rootPath, "Config");
            info.DownloadFolder = Path.Combine(rootPath, "Downloads");
            info.SettingsPath = Path.Combine(rootPath, "Config", "Settings.cfg");
        });
    }
}