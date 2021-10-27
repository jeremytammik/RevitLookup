using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class ElementPhaseStatuses : Data
    {
        private readonly Element _element;

        public ElementPhaseStatuses(string label, Element element) : base(label)
        {
            _element = element;
        }

        public override string StrValue() => "< phases statuses >";

        public override bool HasDrillDown => !_element.Document.Phases.IsEmpty;

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown)
                return null;

            var elementOnPhaseStatuses = _element
                .Document
                .Phases
                .Cast<Phase>()
                .Select(x => new SnoopableObjectWrapper(x.Name, _element.GetPhaseStatus(x.Id)))
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