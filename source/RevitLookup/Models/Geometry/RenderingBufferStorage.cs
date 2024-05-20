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
    public bool HasInvalidGeometry { get; set; }

    public bool IsUpdateRequire()
    {
        if (HasInvalidGeometry) return false;
        if (VertexBuffer is null) return true;
        if (IndexBuffer is null) return true;
        if (VertexFormat is null) return true;
        if (EffectInstance is null) return true;
        if (!VertexBuffer.IsValid()) return true;
        if (!IndexBuffer.IsValid()) return true;
        if (!VertexFormat.IsValid()) return true;
        if (!EffectInstance.IsValid()) return true;

        return false;
    }
}