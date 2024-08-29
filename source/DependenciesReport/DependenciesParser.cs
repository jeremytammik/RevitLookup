namespace DependenciesReport;

public static class DependenciesParser
{
    public static Dictionary<string, Dictionary<string, string>> CreateDependenciesMap(string[] directories)
    {
        var versionMap = new Dictionary<string, Dictionary<string, string>>();
        foreach (var directory in directories)
        {
            var assemblies = Directory.GetFiles(directory, "*.dll");

            foreach (var assembly in assemblies)
            {
                var assemblyName = Path.GetFileName(assembly);
                var assemblyVersion = AssemblyUtils.GetAssemblyVersion(assembly);

                if (!versionMap.TryGetValue(assemblyName, out var value))
                {
                    value = new Dictionary<string, string>();
                    versionMap[assemblyName] = value;
                }

                if (assemblyVersion != null)
                {
                    value[Path.GetFileName(directory)] = assemblyVersion;
                }
            }
        }
        
        return versionMap;
    }
}