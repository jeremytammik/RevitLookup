using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ViewGetNonControlledTemplateParameterIds : Data
    {
        private readonly View _view;

        public ViewGetNonControlledTemplateParameterIds(string label, View view) : base(label)
        {
            _view = view;
        }

        public override string StrValue()
        {
            return "< view non controlled template parameter ids >";
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetNonControlledTemplateParameterIds().Any();

        public override void DrillDown()
        {
            if (!HasDrillDown) return;

            var viewParams = _view.Parameters.Cast<Parameter>().ToList();

            var nonControlledTemplateParameterIds = 
                (from id in _view.GetNonControlledTemplateParameterIds() 
                select viewParams.Find(q => q.Id.IntegerValue == id.IntegerValue) 
                into p where p != null select new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

            if (!nonControlledTemplateParameterIds.Any()) return;

            var form = new Forms.Objects(nonControlledTemplateParameterIds);
            form.ShowDialog();
        }
    }
}