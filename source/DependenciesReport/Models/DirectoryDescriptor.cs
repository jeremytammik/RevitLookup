namespace DependenciesReport.Models;

public sealed record DirectoryDescriptor(string Name, string Path)
{
    public List<AssemblyDescriptor> Assemblies { get; init; } = [];
}