using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core.RevitTypes;

public class ViewGetNonControlledTemplateParameterIdsData : Data
{
    private readonly View _view;

    public ViewGetNonControlledTemplateParameterIdsData(string label, View view) : base(label)
    {
        _view = view;
    }

    public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetNonControlledTemplateParameterIds().Count > 0;

    public override string AsValueString()
    {
        return "< view non controlled template parameter ids >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var nonControlledTemplateParameterIds = _view.GetNonControlledTemplateParameterIds()
            .Select(id => _view.Parameters
                .Cast<Parameter>()
                .ToList()
                .Find(parameter => parameter.Id == id))
            .Where(p => p is not null)
            .Select(p => new SnoopableWrapper(p.Definition.Name, p)).ToList();

        if (nonControlledTemplateParameterIds.Count == 0) return null;

        var form = new ObjectsView(nonControlledTemplateParameterIds);
        return form;
    }
}