using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes
{
    public class TableDataSectionData : Data
    {
        private readonly TableData _tableData;

        public TableDataSectionData(string label, TableData tableData) : base(label)
        {
            _tableData = tableData;
        }

        public override bool HasDrillDown => _tableData is {NumberOfSections: > 0};

        public override string StrValue()
        {
            return "< Get Section Data >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var sectionDataObjects = new List<SnoopableObjectWrapper>();

            foreach (SectionType type in Enum.GetValues(typeof(SectionType)))
            {
                var sectionData = _tableData.GetSectionData(type);
                if (sectionData is null) continue;
                sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), sectionData));
            }

            if (sectionDataObjects.Count == 0) return null;

            var form = new Objects(sectionDataObjects);
            return form;
        }
    }
}