using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using DependenciesReport;
using DependenciesReport.Models;

var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var machineFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
var userAddinsPath = Path.Combine(userFolder, "Autodesk", "Revit", "Addins");
var machineAddinsPath = Path.Combine(machineFolder, "Autodesk", "Revit", "Addins");
var yearsGroups = Directory.GetDirectories(userAddinsPath)
    .Union(Directory.GetDirectories(machineAddinsPath))
    .GroupBy(Path.GetFileName)
    .ToArray();

var reportName = $"DependenciesReport-{DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}.txt";
var reportPath = Path.Combine(Path.GetTempPath(), reportName);
var summaryWriter = new SummaryWriter(reportPath);

var dependenciesMaps = new Dictionary<string, List<DirectoryDescriptor>>();
foreach (var yearsGroup in yearsGroups)
{
    if (yearsGroup.Key is null) continue;
    
    summaryWriter.Write($"Revit version: {Path.GetFileName(yearsGroup.Key)}");
    summaryWriter.WriteLine();

    var directories = yearsGroup.SelectMany(Directory.GetDirectories).ToArray();
    var dependenciesMap = DependenciesTools.CreateDependenciesMap(directories);
    var dependenciesTable = TableFormater.CreateReportTable(dependenciesMap);

    if (dependenciesTable.Rows.Count == 0)
    {
        summaryWriter.Write("No conflicts detected.");
    }
    else
    {
        dependenciesMaps.Add(yearsGroup.Key, dependenciesMap);
        summaryWriter.Write(dependenciesTable.ToMinimalString());
    }

    summaryWriter.WriteLine();
}

summaryWriter.Save();

Console.WriteLine($"Dependencies report saved: {reportPath}");
Console.WriteLine(@"Open the report? Y\N");
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

Console.WriteLine(@"Try to fix dependencies and upgrade to the latest version? Y\N");
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