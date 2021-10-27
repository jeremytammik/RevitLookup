﻿using System;
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

        public override bool HasDrillDown => _tableData is {NumberOfSections: > 0};

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var sectionDataObjects = new List<SnoopableObjectWrapper>();

            foreach (SectionType type in Enum.GetValues(typeof(SectionType)))
            {
                var sectionData = _tableData.GetSectionData(type);
                if (sectionData != null)
                    sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), sectionData));
            }

            if (!sectionDataObjects.Any()) return null;

            var form = new Forms.Objects(sectionDataObjects);
            return form;
        }
    }
}