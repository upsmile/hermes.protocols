using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hermes.Protocol.Gpx.Core.Contracts;
using Hermes.Protocol.Gpx.Protocols;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Hermes.Protocol.Gpx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpxController : ControllerBase
    {
        private IConfiguration Configuration { get; set; }

        public GpxController(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost]
        public async Task<JsonResult> Post() => await Task.Run(() =>
                                                          {
                                                              Request.Headers.TryGetValue("transport_id", out var transportId);
                                                              Request.Headers.TryGetValue("transport_type", out var transportType);
                                                              Request.Headers.TryGetValue("event_date", out var eventDate);
                                                              Request.Headers.TryGetValue("x-correlation-id", out var correlation);
                                                              var de = DateTime.FromFileTime(long.Parse(eventDate.ToList()[0]));
                                                              var cid = correlation.ToList()[0];
                                                              var id = transportId.ToList()[0];
                                                              var tt = transportType.ToList()[0];

                                                              var seq = Configuration["seq"];
                                                              var sentry = Configuration["sentry"];                                                              
                                                              var logger = LoggerBootstrap.CreateLogger(id, tt, de, cid,seq,sentry);
                                                              logger.Information("begin create request {method}", Request.Method);
                                                              try
                                                              {
                                                                  var body = Request.Body;
                                                                  body.Flush();
                                                                  var data = new Core.Contracts.ProtocolContext()
                                                                  {
                                                                      FileByteStream = body,
                                                                      Context = $"{id}#{tt}#{de.ToFileTime()}"
                                                                  };
                                                                  var protocol = new HermesGpxProtocol(logger, Configuration);
                                                                  var result = new Dictionary<string, object>();
                                                                  protocol.Report += (sender, arg) =>
                                                                      {
                                                                          logger.Debug("report successfully complete?");
                                                                          arg.With(x => x.Exception.Do(e =>
                                                                              {
                                                                                  logger.Warning(e, e.Message);
                                                                              }));
                                                                          arg.With(x => x.Result.Do(res =>
                                                                          {
                                                                              result.Add("report",res);
                                                                          }));
                                                                      };
                                                                  protocol.Parsed += (sender, arg) =>
                                                                  {
                                                                      logger.Debug("parser successfully complete?");
                                                                      arg.With(x => x.Exception.Do(e => throw e));
                                                                      arg.With(x => x.Result.Do(res =>
                                                                      {
                                                                          result.Add("report",res);
                                                                      }));
                                                                  };

                                                                  protocol.Posted += (sender, arg) =>
                                                                  {
                                                                      logger.Debug("parser successfully complete?");
                                                                      arg.With(x => x.Exception.Do(e => throw e));
                                                                      arg.With(x => x.Result.Do(res =>
                                                                      {
                                                                          result.Add("report",res);
                                                                      }));
                                                                  };                                                                  
                                                                  protocol.GetMessage(data);                                                                  
                                                                  return new JsonResult(cid);
                                                              }
                                                              catch (Exception e)
                                                              {
                                                                  logger.Fatal(e, e.Message);
                                                                  throw new HttpRequestException(e.Message);
                                                              }
                                                              finally
                                                              {
                                                                  Log.CloseAndFlush();
                                                              }
                                                          });

        [HttpGet]
        public async Task<JsonResult> Get() => await Task.Run(() =>
        {
            var cnf = Configuration["api"];
            var seq = Configuration["seq"];
            var sentry = Configuration["sentry"];                                                              
            
            var logger = LoggerBootstrap.CreateLogger("GET_TEST", "GET_TEST", DateTime.Now, Guid.NewGuid().ToString(), seq, sentry);
            logger.Information("create response =>request {method}", Request.Method);
            
            var result = new
            {
                code = HttpStatusCode.OK,
                message = "hermes.api.protocol"
            };
            return new JsonResult(result);
        });
    }
}