using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using ConsoleTables;
using DependenciesReport;
using DependenciesReport.Models;

var addinLocations = AddinUtilities.GetAddinLocations();
var reportName = $"DependenciesReport-{DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}.txt";
var reportPath = Path.Combine(Path.GetTempPath(), reportName);
var summaryWriter = new SummaryWriter(reportPath);

var dependenciesMaps = new List<List<DirectoryDescriptor>>();
foreach (var addinLocation in addinLocations)
{
    summaryWriter.Write($"### Revit version: {Path.GetFileName(addinLocation.Key)} ###");
    summaryWriter.WriteLine();

    var addinDirectories = AddinUtilities.GetAddinDirectories(addinLocation);
    var dependenciesMap = DependenciesTools.CreateDependenciesMap(addinDirectories);
    var dependenciesTable = TableFormater.CreateReportTable(dependenciesMap);

    if (dependenciesTable.Rows.Count == 0)
    {
        summaryWriter.Write("No conflicts detected.");
        summaryWriter.WriteLine();
    }
    else
    {
        summaryWriter.Write(dependenciesTable.ToMinimalString());
    }

    dependenciesMaps.Add(dependenciesMap);
}

summaryWriter.Write("### Detailed user assemblies summary ###");
summaryWriter.WriteLine();

foreach (var dependenciesMap in dependenciesMaps)
{
    var assembliesTable = TableFormater.CreateAssembliesTable(dependenciesMap);
    if (assembliesTable.Rows.Count > 0)
    {
        summaryWriter.Write(assembliesTable.ToMinimalString());
    }
}

summaryWriter.Save();

Console.WriteLine($"Dependencies report saved: {reportPath}");
Console.WriteLine("Open the report? Y/N");
var response = Console.ReadLine();

if (string.Compare(response, "Y", StringComparison.OrdinalIgnoreCase) == 0)
{
    Process.Start(new ProcessStartInfo
    {
        FileName = "explorer.exe",
        Arguments = reportPath,
        CreateNoWindow = true,
        UseShellExecute = true
    });
}

Console.WriteLine("Try to fix dependencies and upgrade to the latest version? Y/N");
response = Console.ReadLine();

if (string.Compare(response, "Y", StringComparison.OrdinalIgnoreCase) == 0)
{
    var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

    if (!isAdmin)
    {
        Console.WriteLine("Permissions denied, only user dependencies will be updated. For a complete update, run application as administrator.");
    }

    DependenciesTools.UpgradeDependencies(dependenciesMaps);
}

Console.WriteLine();
Console.WriteLine("You can now close this terminal with Ctrl+C");
Console.ReadLine();