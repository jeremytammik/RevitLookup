using System.Reflection;

namespace DependenciesReport;

public static class AssemblyUtils
{
    public static string? GetAssemblyVersion(string dllPath)
    {
        var versionInfo = AssemblyName.GetAssemblyName(dllPath).Version;
        return versionInfo?.ToString(3);
    }
}