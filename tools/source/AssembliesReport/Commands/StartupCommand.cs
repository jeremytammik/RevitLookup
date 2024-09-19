using System.Diagnostics;
using System.Globalization;
using System.IO;
using AssembliesReport.Utilities;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using ConsoleTableExt;

namespace AssembliesReport.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[Transaction(TransactionMode.Manual)]
public class StartupCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var reportPath = Path.Combine(Path.GetTempPath(), $"AssembliesReport-{DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}.txt");
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var tableData = assemblies
            .Where(assembly => !assembly.IsDynamic)
            .Select(AssemblyUtilities.ScanAssembly)
            .Where(summary => summary is not null)
            .ToList();

        var summary = ConsoleTableBuilder.From(tableData)
            .WithColumn("Load order", "Assembly", "Path", "Version", "Domain")
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .Export();

        File.WriteAllText(reportPath, summary.ToString());
        Process.Start("explorer.exe", $"/select,{reportPath}");

        return Result.Succeeded;
    }
}