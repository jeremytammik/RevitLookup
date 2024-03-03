using System.Diagnostics;
using System.Text;

namespace RevitLookup.Utils;

public static class ProcessTasks
{
    public static Process StartProcess(string toolPath, string arguments = "", Action<OutputType, string> logger = null)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = toolPath,
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        
        var process = Process.Start(startInfo)!;
        if (process == null) return null;
        
        RedirectProcessOutput(process, logger);
        return process;
    }
    
    public static Process StartShell(string toolPath, string arguments = "")
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = toolPath,
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = true
        };
        
        return Process.Start(startInfo)!;
    }
    
    private static void RedirectProcessOutput(Process process, Action<OutputType, string> logger)
    {
        logger ??= DefaultLogger;
        process.OutputDataReceived += (_, args) =>
        {
            if (string.IsNullOrEmpty(args.Data)) return;
            logger.Invoke(OutputType.Standard, args.Data);
        };
        process.ErrorDataReceived += (_, args) =>
        {
            if (string.IsNullOrEmpty(args.Data)) return;
            logger.Invoke(OutputType.Error, args.Data);
        };
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
    }
    
    private static void DefaultLogger(OutputType type, string output)
    {
        Console.WriteLine(output);
    }
}

public enum OutputType
{
    Standard,
    Error
}