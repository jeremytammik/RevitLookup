using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

public class ViewGetTemplateParameterIds : Data
{
    private readonly View _view;

    public ViewGetTemplateParameterIds(string label, View view) : base(label)
    {
        _view = view;
    }

    public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetTemplateParameterIds().Count > 0;

    public override string StrValue()
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
            .Select(p => new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

        if (templateParameterIds.Count == 0) return null;

        var form = new Objects(templateParameterIds);
        return form;
    }
}