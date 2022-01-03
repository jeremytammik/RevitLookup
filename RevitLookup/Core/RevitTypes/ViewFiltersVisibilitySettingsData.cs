using RevitLookup.Views;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core.RevitTypes;

public class ViewFiltersVisibilitySettingsData : Data
{
    private readonly View _view;

    public ViewFiltersVisibilitySettingsData(string label, View view) : base(label)
    {
        _view = view;
    }

    public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.AreGraphicsOverridesAllowed() && _view.GetFilters().Count > 0;

    public override string AsValueString()
    {
        return "< view filters visibility >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var filtersVisibility = _view
            .GetFilters()
            .Select(x => new SnoopableWrapper(_view.Document.GetElement(x).Name, _view.GetFilterVisibility(x)))
            .ToList();

        if (filtersVisibility.Count == 0) return null;
        var form = new Objects(filtersVisibility);
        return form;
    }
}