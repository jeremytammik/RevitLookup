using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace RevitLookup.Config;

public static class LoggerConfigurator
{
    public static void AddLoggerConfiguration(this ILoggingBuilder logging)
    {
        logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.IncludeScopes = true;
            options.ColorBehavior = LoggerColorBehavior.Enabled;
        });
    }
}