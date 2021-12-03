using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Core;
using RevitLookup.Views;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     SnoopDB command:  Browse all Elements in the current Document
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SnoopDbCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopDb);
            return Result.Succeeded;
        }
    }
}