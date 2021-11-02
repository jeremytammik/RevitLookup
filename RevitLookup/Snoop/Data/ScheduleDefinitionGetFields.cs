using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Forms;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Snoop.Data
{
    public class ScheduleDefinitionGetFields : Data
    {
        private readonly ScheduleDefinition _scheduleDefinition;

        public ScheduleDefinitionGetFields(string label, ScheduleDefinition scheduleDefinition) : base(label)
        {
            _scheduleDefinition = scheduleDefinition;
        }

        public override bool HasDrillDown => _scheduleDefinition != null && _scheduleDefinition.GetFieldCount() > 0;

        public override string StrValue()
        {
            return "< Get Fields >";
        }

        public override Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var scheduleFieldObjects = new List<SnoopableObjectWrapper>();

            for (var i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
            {
                var field = _scheduleDefinition.GetField(i);
                scheduleFieldObjects.Add(new SnoopableObjectWrapper($"[{i}] {field.GetName()}", field));
            }

            if (!scheduleFieldObjects.Any()) return null;

            var form = new Objects(scheduleFieldObjects);
            return form;
        }
    }
}