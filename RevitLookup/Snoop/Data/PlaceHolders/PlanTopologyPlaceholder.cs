using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data.PlaceHolders
{
    internal class PlanTopologyPlaceholder : IObjectToSnoopPlaceholder
    {
        private readonly Phase _phase;
        private readonly Level _level;


        public PlanTopologyPlaceholder(PlanTopology planTopology)
        {
            _phase = planTopology.Phase;
            _level = planTopology.Level;
        }

        public string GetName()
        {
            return $"PlanTopology<{_level?.Name}, {_phase?.Name}>";
        }

        public object GetObject(Document document)
        {
            return document.get_PlanTopology(_level, _phase);
        }

        public Type GetUnderlyingType()
        {
            return typeof(PlanTopology);
        }
    }
}