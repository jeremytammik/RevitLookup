using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes;

public class ScheduleDefinitionGetFieldsData : Data
{
    private readonly ScheduleDefinition _scheduleDefinition;

    public ScheduleDefinitionGetFieldsData(string label, ScheduleDefinition scheduleDefinition) : base(label)
    {
        _scheduleDefinition = scheduleDefinition;
    }

    public override bool HasDrillDown => _scheduleDefinition is not null && _scheduleDefinition.GetFieldCount() > 0;

    public override string AsValueString()
    {
        return "< Get Fields >";
    }

    public override object DrillDown()
    {
        if (!HasDrillDown) return null;

        var scheduleFieldObjects = new List<SnoopableWrapper>();

        for (var i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
        {
            var field = _scheduleDefinition.GetField(i);
            scheduleFieldObjects.Add(new SnoopableWrapper($"[{i}] {field.GetName()}", field));
        }

        if (scheduleFieldObjects.Count == 0) return null;

        return scheduleFieldObjects;
    }
}