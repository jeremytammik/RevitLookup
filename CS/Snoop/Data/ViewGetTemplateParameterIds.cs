using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ViewGetTemplateParameterIds : Data
    {
        private readonly View _view;

        public ViewGetTemplateParameterIds(string label, View view) : base(label)
        {
            _view = view;
        }

        public override string StrValue()
        {
            return "< view template parameter ids >";
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetTemplateParameterIds().Any();

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var viewParams = _view.Parameters.Cast<Parameter>().ToList();

            var templateParameterIds = 
                (from id in _view.GetTemplateParameterIds() 
                select viewParams.Find(q => q.Id.IntegerValue == id.IntegerValue) 
                into p where p != null select new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

            if (!templateParameterIds.Any()) return null;

            var form = new Forms.Objects(templateParameterIds);
            return form;
        }
    }
}