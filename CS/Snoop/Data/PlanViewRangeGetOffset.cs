using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class PlanViewRangeGetOffset : Data
    {
        private readonly PlanViewRange _planViewRange;

        public PlanViewRangeGetOffset(string label, PlanViewRange planViewRange) : base(label)
        {
            _planViewRange = planViewRange;
        }

        public override string StrValue()
        {
            return "< Get Offsets >";
        }

        public override bool HasDrillDown => _planViewRange != null;

        public override void DrillDown(System.Windows.Forms.Form parent)
        {
            if (!HasDrillDown) return;

            var sectionDataObjects = new List<SnoopableObjectWrapper>();

            foreach (PlanViewPlane type in Enum.GetValues(typeof(PlanViewPlane)))
            {
                var offset = _planViewRange.GetOffset(type);
                sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), offset));
            }

            if (!sectionDataObjects.Any()) return;

            var form = new Forms.Objects(sectionDataObjects);
            ModelessWindowFactory.Show(form, parent);
        }
    }
}