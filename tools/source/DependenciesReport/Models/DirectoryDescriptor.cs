namespace DependenciesReport.Models;

public sealed record DirectoryDescriptor(string Name, string Path, string Version)
{
    public List<AssemblyDescriptor> Assemblies { get; init; } = [];
}