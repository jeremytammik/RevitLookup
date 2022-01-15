using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core.RevitTypes;

public class ViewGetTemplateParameterIdsData : Data
{
    private readonly View _view;

    public ViewGetTemplateParameterIdsData(string label, View view) : base(label)
    {
        _view = view;
    }

    public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetTemplateParameterIds().Count > 0;

    public override string AsValueString()
    {
        return "< view template parameter ids >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var templateParameterIds = _view.GetTemplateParameterIds()
            .Select(id => _view.Parameters
                .Cast<Parameter>()
                .ToList()
                .Find(q => q.Id.IntegerValue == id.IntegerValue))
            .Where(p => p is not null)
            .Select(p => new SnoopableWrapper(p.Definition.Name, p)).ToList();

        if (templateParameterIds.Count == 0) return null;

        var form = new ObjectsView(templateParameterIds);
        return form;
    }
}