using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class TableDataSectionData : Data
    {
        private readonly TableData _tableData;

        public TableDataSectionData(string label, TableData tableData) : base(label)
        {
            _tableData = tableData;
        }

        public override string StrValue()
        {
            return "< Get Section Data >";
        }

        public override bool HasDrillDown => _tableData != null && _tableData.NumberOfSections > 0;

        public override void DrillDown()
        {
            if (!HasDrillDown) return;

            var sectionDataObjects = new List<SnoopableObjectWrapper>();

            foreach (SectionType type in Enum.GetValues(typeof(SectionType)))
            {
                var sectionData = _tableData.GetSectionData(type);
                if (sectionData != null)
                    sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), sectionData));
            }

            if (!sectionDataObjects.Any()) return;

            var form = new Forms.Objects(sectionDataObjects);
            form.ShowDialog();
        }
    }
}