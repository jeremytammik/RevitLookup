﻿using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Forms;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Snoop.Data
{
    public class ViewFiltersVisibilitySettings : Data
    {
        private readonly View _view;

        public ViewFiltersVisibilitySettings(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.AreGraphicsOverridesAllowed() && _view.GetFilters().Any();

        public override string StrValue()
        {
            return "< view filters visibility >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown)
                return null;

            var filtersVisibility = _view
                .GetFilters()
                .Select(x => new SnoopableObjectWrapper(_view.Document.GetElement(x).Name, _view.GetFilterVisibility(x)))
                .ToList();

            if (filtersVisibility.Any())
            {
                var form = new Objects(filtersVisibility);
                return form;
            }

            return null;
        }
    }
}