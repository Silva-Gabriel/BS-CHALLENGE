using Microsoft.Extensions.Logging;

namespace crosscutting.dependencyInjection
{
    public static class LoggingConfigExtension
    {
        public static ILoggingBuilder AddDefaultLogging(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConsole();
            // aqui você poderia plugar Serilog, ApplicationInsights, etc.
            return logging;
        }

    }
}