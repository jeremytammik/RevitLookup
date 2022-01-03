using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes.PlaceHolders;

internal class PlanTopologyPlaceholder : ISnoopPlaceholder
{
    private readonly Level _level;
    private readonly Phase _phase;


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