using System.Diagnostics;
using System.Globalization;
using DependenciesReport;

var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var addinsPath = Path.Combine(userFolder, "Autodesk", "Revit", "Addins");
var yearsDirectories = Directory.GetDirectories(addinsPath);

var reportName = $"Dependencies report - {DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo)}.txt";
var reportPath = Path.Combine(Path.GetTempPath(), reportName);
var summaryWriter = new SummaryWriter(reportPath);

foreach (var yearDirectory in yearsDirectories)
{
    summaryWriter.Write($"Revit version: {Path.GetFileName(yearDirectory)}");
    summaryWriter.WriteLine();
    
    var directories = Directory.GetDirectories(yearDirectory);
    var dependenciesMap = DependenciesParser.CreateDependenciesMap(directories);
    var dependenciesTable = TableFormater.CreateReportTable(directories, dependenciesMap);
    
    summaryWriter.Write(dependenciesTable.ToMinimalString());
    summaryWriter.WriteLine();
}

summaryWriter.Save();
Process.Start("explorer.exe", reportPath);