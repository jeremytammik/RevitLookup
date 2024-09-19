using System.Diagnostics;
using System.Reflection;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace AssembliesReport.Utilities;

public static class AssemblyUtilities
{
    public static List<object> ScanAssembly(Assembly assembly, int index)
    {
        try
        {
            if (assembly.Location != string.Empty)
            {
                var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return
                [
                    index + 1,
                    assembly.GetName().Name,
                    assembly.Location,
                    new Version(fileInfo.FileMajorPart, fileInfo.FileMinorPart, fileInfo.FileBuildPart, fileInfo.FilePrivatePart),
#if NETCOREAPP
                    AssemblyLoadContext.GetLoadContext(assembly)?.Name ?? string.Empty
#else
                    AppDomain.CurrentDomain.FriendlyName
#endif
                ];
            }
        }
        catch
        {
            //ignored
        }

        return null;
    }
}