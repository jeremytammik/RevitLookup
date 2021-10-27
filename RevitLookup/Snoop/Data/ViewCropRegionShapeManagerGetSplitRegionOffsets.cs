using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ViewCropRegionShapeManagerGetSplitRegionOffsets : Data
    {
        private readonly ViewCropRegionShapeManager _viewCropRegionShapeManager;

        public ViewCropRegionShapeManagerGetSplitRegionOffsets(string label, ViewCropRegionShapeManager viewCropRegionShapeManager) : base(label)
        {
            _viewCropRegionShapeManager = viewCropRegionShapeManager;
        }

        public override string StrValue()
        {
            return "< Split Region Offsets >";
        }

        public override bool HasDrillDown => _viewCropRegionShapeManager != null && _viewCropRegionShapeManager.NumberOfSplitRegions > 1;

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var cropRegionOffsetObjects = new List<SnoopableObjectWrapper>();

            for (var i = 0; i < _viewCropRegionShapeManager.NumberOfSplitRegions; i++)
                cropRegionOffsetObjects.Add(new SnoopableObjectWrapper("[" + i + "]", _viewCropRegionShapeManager.GetSplitRegionOffset(i)));

            if (!cropRegionOffsetObjects.Any()) return null;

            var form = new Forms.Objects(cropRegionOffsetObjects);
            return form;
        }
    }
}