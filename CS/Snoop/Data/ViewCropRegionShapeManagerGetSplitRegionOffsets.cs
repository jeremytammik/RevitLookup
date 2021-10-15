﻿using System.Collections.Generic;
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

        public override void DrillDown(System.Windows.Forms.Form parent)
        {
            if (!HasDrillDown) return;

            List<SnoopableObjectWrapper> cropRegionOffsetObjects = new List<SnoopableObjectWrapper>();

            for (int i = 0; i < _viewCropRegionShapeManager.NumberOfSplitRegions; i++)
                cropRegionOffsetObjects.Add(new SnoopableObjectWrapper("[" + i + "]", _viewCropRegionShapeManager.GetSplitRegionOffset(i)));

            if (!cropRegionOffsetObjects.Any()) return;

            var form = new Forms.Objects(cropRegionOffsetObjects);
            ModelessWindowFactory.Show(form, parent);
        }
    }
}