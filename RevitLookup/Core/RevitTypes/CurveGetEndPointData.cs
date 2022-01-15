using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

public class CurveGetEndPointData : Data
{
    private readonly Curve _curve;

    public CurveGetEndPointData(string label, Curve curve) : base(label)
    {
        _curve = curve;

        //ISSUE: Internal exception. Object unloaded from memory
        var _ = _curve.IsBound;
    }

    public override bool HasDrillDown => _curve is not null && _curve.IsBound;

    public override string AsValueString()
    {
        return "< Get End Points >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var xyzObjects = new List<SnoopableWrapper>
        {
            new("[0] Start", _curve.GetEndPoint(0)),
            new("[1] End", _curve.GetEndPoint(1))
        };

        if (xyzObjects.Count == 0) return null;

        var form = new ObjectsView(xyzObjects);
        return form;
    }
}