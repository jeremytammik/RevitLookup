using System.Text;

namespace DependenciesReport.Formatters;

public sealed class SummaryWriter(string reportPath)
{
    private readonly StringBuilder _reportBuilder = new();
    
    public void Write(string message)
    {
        _reportBuilder.AppendLine(message);
    }
    
    public void WriteLine()
    {
        _reportBuilder.AppendLine();
    }
    
    public void Save()
    {
        var summary = _reportBuilder.ToString();
        File.WriteAllText(reportPath, summary);
    }
}