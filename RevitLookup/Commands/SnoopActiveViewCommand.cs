using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Forms;
using RevitLookup.Snoop;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     SnoopDB command:  Browse the current view...
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SnoopActiveViewCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopActiveView);
            return Result.Succeeded;
        }
    }
}