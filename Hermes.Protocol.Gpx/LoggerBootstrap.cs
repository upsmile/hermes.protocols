using System;
using Serilog;
using Serilog.Events;

namespace Hermes.Protocol.Gpx
{
    public static class LoggerBootstrap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="de"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
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
                .WriteTo.Async(a => { a.Console(LogEventLevel.Debug); })
                .CreateLogger();
            return logger;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="de"></param>
        /// <param name="cid"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static ILogger CreateLogger(string id, string type, DateTime de, string cid, string seq, string sentry)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("protocol", "hermes.gpx.protocol")
                .Enrich.WithProperty("transport-id", id)
                .Enrich.WithProperty("transport-type", type)
                .Enrich.WithProperty("event-date", de)
                .Enrich.WithProperty("correlation-id", cid)
                .WriteTo.Async(a => { a.Debug(LogEventLevel.Debug); })
                .WriteTo.Async(a => { a.Console(LogEventLevel.Debug); })
                .Enrich.FromLogContext()
                .WriteTo.Async(a => { a.Sentry(sentry);})
                .WriteTo.Async(a => { a.Seq(seq);})
                .CreateLogger();
            return logger;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static ILogger CreateLogger(string cid)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("protocol", "hermes.gpx.protocol")
                .Enrich.WithProperty("correlation-id", cid)
                .WriteTo.Async(a => { a.Debug(LogEventLevel.Debug); })
                .WriteTo.Async(a => { a.Console(LogEventLevel.Debug); })
                .CreateLogger();
            return logger;
        }
    }
}