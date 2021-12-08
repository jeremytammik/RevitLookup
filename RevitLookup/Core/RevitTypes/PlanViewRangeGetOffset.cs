using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

public class PlanViewRangeGetOffset : Data
{
    private readonly PlanViewRange _planViewRange;

    public PlanViewRangeGetOffset(string label, PlanViewRange planViewRange) : base(label)
    {
        _planViewRange = planViewRange;
    }

    public override bool HasDrillDown => _planViewRange is not null;

    public override string StrValue()
    {
        return "< Get Offsets >";
    }

    public override Form DrillDown()
    {
        if (!HasDrillDown) return null;

        var sectionDataObjects = new List<SnoopableObjectWrapper>();

        foreach (PlanViewPlane type in Enum.GetValues(typeof(PlanViewPlane)))
        {
            var offset = _planViewRange.GetOffset(type);
            sectionDataObjects.Add(new SnoopableObjectWrapper(type.ToString(), offset));
        }

        if (sectionDataObjects.Count == 0) return null;

        var form = new Objects(sectionDataObjects);
        return form;
    }
}