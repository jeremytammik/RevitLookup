using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Models.Options;
using RevitLookup.Utils;

namespace RevitLookup.Config;

public static class OptionsConfiguration
{
    public static void AddOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var rootPath = configuration.GetValue<string>("contentRoot");
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var fileVersion = new Version(FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion!);
        
        var targetFrameworkAttribute = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true)
            .Cast<TargetFrameworkAttribute>()
            .First();
        
        services.Configure<AssemblyInfo>(options =>
        {
            options.Framework = targetFrameworkAttribute.FrameworkDisplayName;
            options.AddinVersion = new Version(fileVersion.Major, fileVersion.Minor, fileVersion.Build);
            options.IsAdminInstallation = assemblyLocation.StartsWith(appDataPath) || !AccessUtils.CheckWriteAccess(assemblyLocation);
        });
        
        services.Configure<FolderLocations>(options =>
        {
            options.RootFolder = rootPath;
            options.ConfigFolder = Path.Combine(rootPath, "Config");
            options.DownloadFolder = Path.Combine(rootPath, "Downloads");
            options.GeneralSettingsPath = Path.Combine(rootPath, "Config", "Settings.cfg");
            options.RenderSettingsPath = Path.Combine(rootPath, "Config", "RenderSettings.cfg");
        });
        
        services.Configure<JsonSerializerOptions>(options =>
        {
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.Converters.Add(new JsonStringEnumConverter());
        });
    }
}