using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.Snoop.Data
{
    public class CurveGetEndPoint : Data
    {
        private readonly Curve _curve;

        public CurveGetEndPoint(string label, Curve curve) : base(label)
        {
            _curve = curve;
        }

        public override bool HasDrillDown => _curve != null && _curve.IsBound;

        public override string StrValue()
        {
            return "< Get End Points >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var xyzObjects = new List<SnoopableObjectWrapper>
            {
                new("[0] Start", _curve.GetEndPoint(0)),
                new("[1] End", _curve.GetEndPoint(1))
            };

            if (!xyzObjects.Any()) return null;

            var form = new Objects(xyzObjects);
            return form;
        }
    }
}