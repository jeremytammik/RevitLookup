using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes
{
    public class ViewGetNonControlledTemplateParameterIds : Data
    {
        private readonly View _view;

        public ViewGetNonControlledTemplateParameterIds(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetNonControlledTemplateParameterIds().Count > 0;

        public override string StrValue()
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
                    .Find(q => q.Id.IntegerValue == id.IntegerValue))
                .Where(p => p is not null)
                .Select(p => new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

            if (nonControlledTemplateParameterIds.Count == 0) return null;

            var form = new Objects(nonControlledTemplateParameterIds);
            return form;
        }
    }
}