using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        
        private IConfiguration _configuration { get; set; }

        public GpxController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));            
        }
        
        
        [HttpPost]
        public async Task<JsonResult> Post() => await Task.Run(() =>
                                                          {
                                                              Request.Headers.TryGetValue("transport_id", 
                                                                  out var transportId);
                                                              Request.Headers.TryGetValue("transport_type",
                                                                  out var transportType);
                                                              Request.Headers.TryGetValue("event_date", 
                                                                  out var eventDate);
                                                              Request.Headers.TryGetValue("x-correlation-id", 
                                                                  out var correlation);

                                                              var de = DateTime.FromFileTime(long.Parse(eventDate.
                                                                  ToList()[0]));
                                                              var cid = correlation.ToList()[0];
                                                              var id = transportId.ToList()[0];
                                                              var tt = transportType.ToList()[0];
                                                              
                                                              var type = tt == "1" ? "Грузовой и прочий" : "TA";
                                                              
                                                              var logger = LoggerBootstrap
                                                                            .CreateLogger(id, tt, de, cid);
                                                              logger.Information("begin create request {method}",
                                                                  Request.Method);
                                                              try
                                                              {
                                                                  var body = Request.Body;
                                                                  body.Flush();
                                                                  var data = new Core.Contracts.ProtocolContext()
                                                                  {
                                                                      FileByteStream = body,
                                                                      Context = $"{id}#{tt}#{de.ToFileTime()}"
                                                                  };
                                                                  var protocol = new HermesGpxProtocol(logger,_configuration);
                                                                  JsonResult result = null;
                                                                  protocol.Posted += (sender, arg) =>
                                                                  {
                                                                      if (arg.Exception != null)
                                                                      {
                                                                          throw arg.Exception;
                                                                      }
                                                                      if (arg.Result != null)
                                                                      {
                                                                          result = new JsonResult(arg.Result);
                                                                      }
                                                                      else
                                                                      {
                                                                          logger.Warning("protocol result is empty");
                                                                          throw new InvalidOperationException
                                                                              ("protocol result is empty");
                                                                      }
                                                                  };
                                                                  protocol.GetMessage(data);
                                                                  return result;
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
        public async Task<JsonResult> Get() => await Task.Run(() =>{
                                                                      var logger = LoggerBootstrap.CreateLogger(Guid.NewGuid().ToString());
                                                                      logger.Information("create response =>request {method}",Request.Method);
                                                                      var result = new
                                                                      {
                                                                          code = HttpStatusCode.OK,
                                                                          message = "hermes.api.protocol"
                                                                      };
                                                                      return new JsonResult(result);
                                                                  });
    }
}