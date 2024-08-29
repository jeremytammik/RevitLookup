using System.Collections;
using ConsoleTables;

namespace DependenciesReport;

public static class TableFormater
{
    public static ConsoleTable CreateReportTable(string[] directories, Dictionary<string, Dictionary<string, string>> dependenciesMap)
    {
        var conflictMap = dependenciesMap.Where(pair => pair.Value.Values.Distinct().Count() > 1);
        var table = new ConsoleTable(directories.Select(Path.GetFileName).Prepend("Library").ToArray());

        foreach (var conflictPair in conflictMap)
        {
            var row = new ArrayList { conflictPair.Key };
            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);
                row.Add(conflictPair.Value.GetValueOrDefault(directoryName, "-"));
            }

            table.AddRow(row.ToArray());
        }
        
        return table;
    }
}