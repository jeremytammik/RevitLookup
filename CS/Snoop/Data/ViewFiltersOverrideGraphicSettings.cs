using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ViewFiltersOverrideGraphicSettings : Data
    {
        private readonly View view;

        public ViewFiltersOverrideGraphicSettings(string label, View view) : base(label)
        {
            this.view = view;
        }

        public override string StrValue()
        {
            return "< view filters ovverride graphic settings >";
        }

        public override bool HasDrillDown => !view.Document.IsFamilyDocument && view.AreGraphicsOverridesAllowed() && view.GetFilters().Any();

        public override void DrillDown()
        {
            if (!HasDrillDown)
                return;

            var filterOverrides = view
                .GetFilters()
                .Select(x => new SnoopableObjectWrapper(view.Document.GetElement(x).Name, view.GetFilterOverrides(x)))
                .ToList();

            if (filterOverrides.Any())
            {
                var form = new Forms.Objects(filterOverrides);

                form.ShowDialog();
            }
        }
    }
}