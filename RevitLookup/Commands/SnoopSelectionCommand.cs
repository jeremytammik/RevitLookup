using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Core.Snoop;
using RevitLookup.Forms;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    ///     In case nothing is selected: browse visible elements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SnoopSelectionCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopCurrentSelection);
            return Result.Succeeded;
        }
    }
}