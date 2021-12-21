using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

public class ViewCropRegionShapeManagerGetSplitRegionOffsets : Data
{
    private readonly ViewCropRegionShapeManager _viewCropRegionShapeManager;

    public ViewCropRegionShapeManagerGetSplitRegionOffsets(string label, ViewCropRegionShapeManager viewCropRegionShapeManager) : base(label)
    {
        _viewCropRegionShapeManager = viewCropRegionShapeManager;
    }

    public override bool HasDrillDown => _viewCropRegionShapeManager is {NumberOfSplitRegions: > 1};

    public override string AsValueString()
    {
        return "< Split Region Offsets >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var cropRegionOffsetObjects = new List<SnoopableObjectWrapper>();

        for (var i = 0; i < _viewCropRegionShapeManager.NumberOfSplitRegions; i++)
            cropRegionOffsetObjects.Add(new SnoopableObjectWrapper($"[{i}]", _viewCropRegionShapeManager.GetSplitRegionOffset(i)));

        if (cropRegionOffsetObjects.Count == 0) return null;

        var form = new Objects(cropRegionOffsetObjects);
        return form;
    }
}