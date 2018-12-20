using System;
using System.IO;
using FakeItEasy;
using FluentAssert;
using Hermes.Protocol.Gpx;
using Hermes.Protocol.Gpx.Controllers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using Xunit;

namespace Hermes.Protocol.Test
{
    public class GpxProtocolUnitTest
    {
        private readonly ILogger _logger;
        
        public GpxProtocolUnitTest()
        {
            _logger = LoggerBootstrap.CreateLogger(Guid.NewGuid().ToString());
        }
        [Theory]
        [InlineData("xml_file")]
        public void PostControllerIntegrationTest(string xmlFile)
        {
            
            var transportId = new StringValues();
            var transportType = new StringValues();
            var eventDate = new StringValues();
            var correlation = new StringValues();                                     
            var context = A.Fake<HttpContext>();            
            var request = context.Request;
            request.Headers.Add("transport_id", transportId);
            request.Headers.Add("transport_type", transportType);
            request.Headers.Add("event_date",eventDate);
            request.Headers.Add("x-correlation-id", correlation);
            request.Body = File.OpenRead(xmlFile);            
            var protocol = new HermesGpxProtocol(_logger);
            protocol.ShouldNotBeNull();

            protocol.Posted += (sender, arg) =>
            {
                arg.Result.ShouldNotBeNull();
                arg.Exception.ShouldBeNull();
            };
            protocol.Post(null);

        }
    }
}
