using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

public class PlanViewRangeGetLevelIdData : Data
{
    private readonly Document _document;
    private readonly PlanViewRange _planViewRange;

    public PlanViewRangeGetLevelIdData(string label, PlanViewRange planViewRange, Document doc) : base(label)
    {
        _planViewRange = planViewRange;
        _document = doc;
    }

    public override bool HasDrillDown => _planViewRange is not null;

    public override string AsValueString()
    {
        return "< Get Level Ids >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var sectionDataObjects = new List<SnoopableWrapper>();

        foreach (PlanViewPlane type in Enum.GetValues(typeof(PlanViewPlane)))
        {
            var levelId = _planViewRange.GetLevelId(type);
            if (levelId is not null && levelId != ElementId.InvalidElementId)
            {
                var level = _document.GetElement(levelId) as Level;
                sectionDataObjects.Add(new SnoopableWrapper(type.ToString(), level));
            }
            else
            {
                sectionDataObjects.Add(new SnoopableWrapper(type.ToString(), levelId));
            }
        }

        if (sectionDataObjects.Count == 0) return null;

        var form = new Objects(sectionDataObjects);
        return form;
    }
}