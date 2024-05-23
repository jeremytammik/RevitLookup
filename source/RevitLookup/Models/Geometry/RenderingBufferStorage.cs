using Autodesk.Revit.DB.DirectContext3D;

namespace RevitLookup.Models.Geometry;

public sealed class RenderingBufferStorage
{
    public List<MeshInfo> Meshes { get; } = [];
    public List<IList<XYZ>> EdgePoints { get; } = [];
    public VertexFormatBits FormatBits { get; set; }
    public int PrimitiveCount { get; set; }
    public int VertexBufferCount { get; set; }
    public int IndexBufferCount { get; set; }
    public VertexBuffer VertexBuffer { get; set; }
    public IndexBuffer IndexBuffer { get; set; }
    public VertexFormat VertexFormat { get; set; }
    public EffectInstance EffectInstance { get; set; }
}