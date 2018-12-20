using System;
using Serilog;
using Serilog.Events;

namespace Hermes.Protocol.Gpx
{
    public static class LoggerBootstrap
    {
        public static ILogger CreateLogger(string id, string type, DateTime de, string cid)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("protocol", "hermes.gpx.protocol")
                .Enrich.WithProperty("transport-id", id)
                .Enrich.WithProperty("transport-type", type)
                .Enrich.WithProperty("event-date", de)
                .Enrich.WithProperty("correlation-id", cid)
                .WriteTo.Async(a => { a.Debug(LogEventLevel.Debug); })
                .WriteTo.Async(a => { a.Console(); })
                .CreateLogger();
            return logger;
        }

        public static ILogger CreateLogger(string cid)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("protocol", "hermes.gpx.protocol")
                .Enrich.WithProperty("correlation-id", cid)
                .WriteTo.Async(a => { a.Debug(LogEventLevel.Debug); })
                .WriteTo.Async(a => { a.Console(); })
                .CreateLogger();
            return logger;
        }
    }
}