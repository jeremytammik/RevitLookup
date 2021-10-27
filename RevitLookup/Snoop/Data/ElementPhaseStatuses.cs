using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ElementPhaseStatuses : Data
    {
        private readonly Element element;

        public ElementPhaseStatuses(string label, Element element) : base(label)
        {
            this.element = element;
        }

        public override string StrValue() => "< phases statuses >";

        public override bool HasDrillDown => !element.Document.Phases.IsEmpty;

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown)
                return null;

            var elementOnPhaseStatuses = element
                .Document
                .Phases
                .Cast<Phase>()
                .Select(x => new SnoopableObjectWrapper(x.Name, element.GetPhaseStatus(x.Id)))
                .ToList();

            if (elementOnPhaseStatuses.Any())
            {
                var form = new Forms.Objects(elementOnPhaseStatuses);
                return form;
            }

            return null;
        }
    }
}