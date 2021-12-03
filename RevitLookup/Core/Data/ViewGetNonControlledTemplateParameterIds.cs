using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.Data
{
    public class ViewGetNonControlledTemplateParameterIds : Data
    {
        private readonly View _view;

        public ViewGetNonControlledTemplateParameterIds(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetNonControlledTemplateParameterIds().Any();

        public override string StrValue()
        {
            return "< view non controlled template parameter ids >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var viewParams = _view.Parameters.Cast<Parameter>().ToList();

            var nonControlledTemplateParameterIds =
                (from id in _view.GetNonControlledTemplateParameterIds()
                    select viewParams.Find(q => q.Id.IntegerValue == id.IntegerValue)
                    into p
                    where p != null
                    select new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

            if (!nonControlledTemplateParameterIds.Any()) return null;

            var form = new Objects(nonControlledTemplateParameterIds);
            return form;
        }
    }
}