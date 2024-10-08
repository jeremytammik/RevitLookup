using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RevitLookup.Config;

public static class LoggerConfigurator
{
    private const string LogTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";

    public static LoggingLevelSwitch AddSerilogConfiguration(this ILoggingBuilder builder, LoggingLevelSwitch loggingLevelSwitch = null)
    {
        loggingLevelSwitch ??= new LoggingLevelSwitch();
        var logger = CreateDefaultLogger(loggingLevelSwitch);
        builder.AddSerilog(logger);

        AppDomain.CurrentDomain.UnhandledException += OnOnUnhandledException;
        return loggingLevelSwitch;
    }

    private static Logger CreateDefaultLogger(LoggingLevelSwitch loggingLevelSwitch)
    {
        return new LoggerConfiguration()
            .WriteTo.Console(LogEventLevel.Information, LogTemplate)
            .WriteTo.Debug(LogEventLevel.Debug, LogTemplate)
            .WriteTo.RevitJournal(Context.UiApplication, restrictedToMinimumLevel: LogEventLevel.Error, outputTemplate: LogTemplate)
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .CreateLogger();
    }

    private static void OnOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        var exception = (Exception) args.ExceptionObject;
        var logger = Host.GetService<ILogger<AppDomain>>();
        logger.LogCritical(exception, "Domain unhandled exception");
    }
}

public enum LogLevel
{
    Verbose = 0,
    Debug = 1,
    Information = 2,
    Warning = 3,
    Error = 4,
    Fatal = 5
}