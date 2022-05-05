using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Streams;

public class SpatialElementStream : IElementStream
{
    private readonly SpatialElementBoundaryOptions _boundaryOptions;
    private readonly List<Data> _data;
    private readonly SpatialElement _spatialElement;

    public SpatialElementStream(List<Data> data, object element)
    {
        _data = data;
        _spatialElement = element as SpatialElement;

        _boundaryOptions = new SpatialElementBoundaryOptions
        {
            StoreFreeBoundaryFaces = true,
            SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center
        };
    }

    public void Stream(Type type)
    {
        if (MustStream(type))
            _data.Add(new ObjectData("GetBoundarySegments", _spatialElement.GetBoundarySegments(_boundaryOptions)));
    }

    private bool MustStream(Type type)
    {
        var typeNames = new[]
        {
            "Space",
            "SpatialElement",
            "Room"
        };
        return _spatialElement is not null && typeNames.Contains(type.Name);
    }
}