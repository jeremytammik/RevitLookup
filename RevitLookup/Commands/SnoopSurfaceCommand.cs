using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Core.Snoop;
using RevitLookup.Forms;

namespace RevitLookup.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SnoopSurfaceCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopPickFace);
            return Result.Succeeded;
        }
    }
}