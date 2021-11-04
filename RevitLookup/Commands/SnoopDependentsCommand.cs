using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Forms;
using RevitLookup.Snoop;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     Snoop dependent elements using
    ///     Element.GetDependentElements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SnoopDependentsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopDependentElements);
            return Result.Succeeded;
        }
    }
}