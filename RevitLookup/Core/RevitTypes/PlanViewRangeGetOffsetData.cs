using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes;

public class PlanViewRangeGetOffsetData : Data
{
    private readonly PlanViewRange _planViewRange;

    public PlanViewRangeGetOffsetData(string label, PlanViewRange planViewRange) : base(label)
    {
        _planViewRange = planViewRange;
    }

    public override bool HasDrillDown => _planViewRange is not null;

    public override string AsValueString()
    {
        return "< Get Offsets >";
    }

    public override object DrillDown()
    {
        if (!HasDrillDown) return null;

        var sectionDataObjects = new List<SnoopableWrapper>();

        foreach (PlanViewPlane type in Enum.GetValues(typeof(PlanViewPlane)))
        {
            var offset = _planViewRange.GetOffset(type);
            sectionDataObjects.Add(new SnoopableWrapper(type.ToString(), offset));
        }

        if (sectionDataObjects.Count == 0) return null;

        return sectionDataObjects;
    }
}