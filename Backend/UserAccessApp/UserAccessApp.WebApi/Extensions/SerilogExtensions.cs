using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace UserAccessApp.WebApi.Extensions
{
    public static class SerilogExtensions
    {
        public static void RegisterSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                    .MinimumLevel.Override("Serilog", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .Enrich.WithUtcTime()
                    .WriteTo.Async(wt => wt.File("./log/log-.txt", rollingInterval: RollingInterval.Day))
                    .WriteTo.Async(wt =>
                        wt.Console(
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level:u3} {ClientIp}] {Message:lj}{NewLine}{Exception}"))
            );
        }

        public static LoggerConfiguration WithUtcTime(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            return enrichmentConfiguration.With<UtcTimestampEnricher>();
        }

        internal class UtcTimestampEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
            {
                logEvent.AddOrUpdateProperty(pf.CreateProperty("TimeStamp", logEvent.Timestamp.UtcDateTime));
            }
        }
    }
}
