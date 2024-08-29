using System.Diagnostics;
using System.Globalization;
using DependenciesReport;

var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var addinsPath = Path.Combine(userFolder, "Autodesk", "Revit", "Addins");
var yearsDirectories = Directory.GetDirectories(addinsPath);

var reportName = $"DependenciesReport-{DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}.txt";
var reportPath = Path.Combine(Path.GetTempPath(), reportName);
var summaryWriter = new SummaryWriter(reportPath);

var dependenciesMaps = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
foreach (var yearDirectory in yearsDirectories)
{
    summaryWriter.Write($"Revit version: {Path.GetFileName(yearDirectory)}");
    summaryWriter.WriteLine();

    var directories = Directory.GetDirectories(yearDirectory);
    var dependenciesMap = DependenciesTools.CreateDependenciesMap(directories);
    var dependenciesTable = TableFormater.CreateReportTable(directories, dependenciesMap);
    dependenciesMaps.Add(yearDirectory, dependenciesMap);

    summaryWriter.Write(dependenciesTable.ToMinimalString());
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
    DependenciesTools.UpgradeDependencies(dependenciesMaps);
    Console.WriteLine("Success!");
}

Console.WriteLine("You can now close this terminal with Ctrl+C");
Console.ReadLine();