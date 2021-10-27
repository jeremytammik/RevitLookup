﻿using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Forms;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Snoop.Data
{
    public class ViewGetTemplateParameterIds : Data
    {
        private readonly View _view;

        public ViewGetTemplateParameterIds(string label, View view) : base(label)
        {
            _view = view;
        }

        public override bool HasDrillDown => !_view.Document.IsFamilyDocument && _view.IsTemplate && _view.GetTemplateParameterIds().Any();

        public override string StrValue()
        {
            return "< view template parameter ids >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var viewParams = _view.Parameters.Cast<Parameter>().ToList();

            var templateParameterIds =
                (from id in _view.GetTemplateParameterIds()
                    select viewParams.Find(q => q.Id.IntegerValue == id.IntegerValue)
                    into p
                    where p != null
                    select new SnoopableObjectWrapper(p.Definition.Name, p)).ToList();

            if (!templateParameterIds.Any()) return null;

            var form = new Objects(templateParameterIds);
            return form;
        }
    }
}