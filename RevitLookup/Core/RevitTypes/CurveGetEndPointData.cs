using Autodesk.Revit.DB;

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

    public override string Value=> "< Get End Points >";

    public override object DrillDown()
    {
        if (!HasDrillDown) return null;

        var xyzObjects = new List<SnoopableWrapper>
        {
            new("[0] Start", _curve.GetEndPoint(0)),
            new("[1] End", _curve.GetEndPoint(1))
        };

        return xyzObjects.Count == 0 ? null : xyzObjects;
    }
}