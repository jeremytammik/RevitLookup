namespace RevitLookup.Models.Geometry;

public sealed class MeshInfo(Mesh mesh, XYZ normal, ColorWithTransparency color)
{
    public readonly ColorWithTransparency ColorWithTransparency = color;
    public readonly Mesh Mesh = mesh;
    public readonly XYZ Normal = normal;
}