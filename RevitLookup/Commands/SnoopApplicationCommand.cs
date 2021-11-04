using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Core.Snoop;
using RevitLookup.Forms;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     Snoop App command:  Browse all objects that are part of the Application object
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SnoopApplicationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopApplication);
            return Result.Succeeded;
        }
    }
}