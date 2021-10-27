using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
    public class SpatialElementStream : IElementStream
    {
        private readonly ArrayList _data;
        private readonly SpatialElement _spatialElement;
        private readonly SpatialElementBoundaryOptions _boundaryOptions;

        public SpatialElementStream(ArrayList data, object elem)
        {
            _data = data;
            _spatialElement = elem as SpatialElement;

            _boundaryOptions = new SpatialElementBoundaryOptions
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center
            };
        }

        public void Stream(Type type)
        {
            if (MustStream(type))
                _data.Add(new Snoop.Data.Object("GetBoundarySegments", _spatialElement.GetBoundarySegments(_boundaryOptions)));
        }

        private bool MustStream(Type type)
        {
            var typeNames = new[]
            {
                "Space",
                "SpatialElement",
                "Room"
            };
            return _spatialElement != null && typeNames.Contains(type.Name);
        }
    }
}