using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes;

public class ElementPhaseStatusesData : Data
{
    private readonly Element _element;

    public ElementPhaseStatusesData(string label, Element element) : base(label)
    {
        _element = element;
    }

    public override bool HasDrillDown => !_element.Document.Phases.IsEmpty;

    public override string AsValueString()
    {
        return "< phases statuses >";
    }

    public override object DrillDown()
    {
        if (!HasDrillDown) return null;

        var elementOnPhaseStatuses = _element
            .Document
            .Phases
            .Cast<Phase>()
            .Select(x => new SnoopableWrapper(x.Name, _element.GetPhaseStatus(x.Id)))
            .ToList();

        return elementOnPhaseStatuses.Count == 0 ? null : elementOnPhaseStatuses;
    }
}