using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class PlanViewRangeGetLevelId : Data
    {
        private readonly PlanViewRange _planViewRange;
        private readonly Document _document;

        public PlanViewRangeGetLevelId(string label, PlanViewRange planViewRange, Document doc) : base(label)
        {
            _planViewRange = planViewRange;
            _document = doc;
        }

        public override string StrValue()
        {
            return "< Get Level Ids >";
        }

        public override bool HasDrillDown => _planViewRange != null;

        public override void DrillDown()
        {
            if (!HasDrillDown) return;

            var sectionDataObjects = new List<SnoopableObjectWrapper>();

            foreach (PlanViewPlane type in Enum.GetValues(typeof(PlanViewPlane)))
            {
                var levelId = _planViewRange.GetLevelId(type);
                if (levelId != null && levelId != Autodesk.Revit.DB.ElementId.InvalidElementId)
                {
                    var level = _document.GetElement(levelId) as Level;
                    sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), level));
                }
                else
                    sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), levelId));
            }

            if (!sectionDataObjects.Any()) return;

            var form = new Forms.Objects(sectionDataObjects);
            form.ShowDialog();
        }
    }
}