using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.Data
{
    public class ViewFiltersOverrideGraphicSettings : Data
    {
        private readonly View _view;

        public ViewFiltersOverrideGraphicSettings(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.AreGraphicsOverridesAllowed() && _view.GetFilters().Any();

        public override string StrValue()
        {
            return "< view filters ovverride graphic settings >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown)
                return null;

            var filterOverrides = _view
                .GetFilters()
                .Select(x => new SnoopableObjectWrapper(_view.Document.GetElement(x).Name, _view.GetFilterOverrides(x)))
                .ToList();

            if (filterOverrides.Any())
            {
                var form = new Objects(filterOverrides);
                return form;
            }

            return null;
        }
    }
}