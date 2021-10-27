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
        private Phase phase;
        private Level level;


        public PlanTopologyPlaceholder(PlanTopology planTopology)
        {
            phase = planTopology.Phase;
            level = planTopology.Level;
        }

        public string GetName()
        {
            return $"PlanTopology<{level?.Name}, {phase?.Name}>";
        }

        public object GetObject(Document document)
        {
            return document.get_PlanTopology(level, phase);
        }

        public Type GetUnderlyingType()
        {
            return typeof(PlanTopology);
        }
    }
}