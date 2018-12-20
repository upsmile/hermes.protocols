using System;
using System.IO;
using System.Linq;
using FakeItEasy;
using FluentAssert;
using Hermes.EntityFrameworkCore.Data.Model;
using Hermes.Protocol.Gpx;
using Hermes.Protocol.Gpx.Controllers.Contracts;
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
        [InlineData("./Data/131890700909312893_1723776026.gpx")]
        public void PostControllerIntegrationTest(string xmlFile)
        {
            
/*          var transportId = new StringValues();
            var transportType = new StringValues();
            var eventDate = new StringValues();
            var correlation = new StringValues();    
            
            request.Headers.Add("transport_id", transportId);
            request.Headers.Add("transport_type", transportType);
            request.Headers.Add("event_date",eventDate);
            request.Headers.Add("x-correlation-id", correlation);
                                             
            
            */          
            var context = A.Fake<HttpContext>();  
            var request = context.Request;

            request.Body = File.OpenRead(xmlFile);           
            var protocol = new HermesGpxProtocol(_logger);
            protocol.ShouldNotBeNull();

            var data = new GpxData
            {
                FileByteStream = request.Body,
                Context = $"{131890700909312893}_{1723776026}.gpx&{1}"
            };
            
            protocol.Posted += (sender, arg) =>
            {
                arg.Exception.ShouldBeNull();
                arg.Result.ShouldNotBeNull();
            };
            protocol.Post(data);
        }

        [Fact(DisplayName = "test mssql connection to database")]
        public void DataContextTest()
        {
            using (var context = new HermesDataContext())
            {
                context.ShouldNotBeNull();
                var list = context.TransportTypes.Select(x => x.TypeName).ToList();
                list.ShouldNotBeNull();
                list.Count.ShouldBeEqualTo(2);
            }            
        }
    }
}
