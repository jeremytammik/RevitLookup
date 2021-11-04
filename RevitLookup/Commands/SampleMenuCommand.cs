using System;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Commands
{
    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SampleMenuCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            Result result;

            try
            {
                MessageBox.Show("Called back into RevitLookup by picking toolbar or menu item");
                result = Result.Succeeded;
            }
            catch (Exception e)
            {
                msg = e.Message;
                result = Result.Failed;
            }

            return result;
        }
    }
}