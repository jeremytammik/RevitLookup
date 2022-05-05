using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes;

public class TableDataSectionData : Data
{
    private readonly TableData _tableData;

    public TableDataSectionData(string label, TableData tableData) : base(label)
    {
        _tableData = tableData;
    }

    public override bool HasDrillDown => _tableData is {NumberOfSections: > 0};

    public override string AsValueString()
    {
        return "< Get Section Data >";
    }

    public override object DrillDown()
    {
        if (!HasDrillDown) return null;

        var sectionDataObjects = new List<SnoopableWrapper>();

        foreach (SectionType type in Enum.GetValues(typeof(SectionType)))
        {
            var sectionData = _tableData.GetSectionData(type);
            if (sectionData is null) continue;
            sectionDataObjects.Add(new SnoopableWrapper(type.ToString(), sectionData));
        }

        if (sectionDataObjects.Count == 0) return null;

        return sectionDataObjects;
    }
}