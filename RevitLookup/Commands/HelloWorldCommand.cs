using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Commands;

/// <summary>
///     The classic bare-bones test.  Just brings up an Alert box to show that the connection to the external module is working.
/// </summary>
[Transaction(TransactionMode.Manual)]
public class HelloWorldCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
    {
        var a = Assembly.GetExecutingAssembly();
        var dialog = new TaskDialog("Autodesk Revit");
        dialog.MainContent = $"Hello World from {a.Location} v{a.GetName().Version}";
        dialog.Show();
        return Result.Cancelled;
    }
}