using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
    public class SpatialElementStream : IElementStream
    {
        private readonly ArrayList data;
        private readonly SpatialElement spatialElement;
        private readonly SpatialElementBoundaryOptions boundaryOptions;

        public SpatialElementStream(ArrayList data, object elem)
        {
            this.data = data;
            spatialElement = elem as SpatialElement;

            boundaryOptions = new SpatialElementBoundaryOptions
            {
                StoreFreeBoundaryFaces = true,
                SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center
            };
        }

        public void Stream(Type type)
        {
            if (MustStream(type))
                data.Add(new Snoop.Data.Object("GetBoundarySegments", spatialElement.GetBoundarySegments(boundaryOptions)));
        }

        private bool MustStream(Type type)
        {
            var typeNames = new[]
            {
                "Space",
                "SpatialElement",
                "Room"
            };
            return spatialElement != null && typeNames.Contains(type.Name);
        }
    }
}