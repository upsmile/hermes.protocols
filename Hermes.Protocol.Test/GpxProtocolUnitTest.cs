using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using FluentAssert;
using Hermes.EntityFrameworkCore.Data.Model;
using Hermes.Protocol.Gpx;
using Hermes.Protocol.Gpx.Core.Services;
using Hermes.Protocol.Gpx.Protocols;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
            //var config = A.Fake<IConfiguration>();
            
            
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"api", "http://193.23.58.37:8086/api/" }                   
                });

            var config =  configurationBuilder.Build();
            
            var request = context.Request;

            request.Body = File.OpenRead(xmlFile);           
            var protocol = new HermesGpxProtocol(_logger, config);
            protocol.ShouldNotBeNull();

            var data = new Gpx.Core.Contracts.ProtocolContext
            {
                FileByteStream = request.Body,
                Context = $"{131890700909312893}_{1723776026}.gpx&{1}"
            };
            
            protocol.Posted += (sender, arg) =>
            {
                arg.Exception.ShouldBeNull();
                arg.Result.ShouldNotBeNull();
            };
            protocol.GetMessage(data);
        }

        [Theory]
        [InlineData("./Data/131890700909312893_1723776026.gpx", "131890700909312893","1723776026")]
        [InlineData("./Data/131544183883686029_201950020.gpx", "131544183883686029", "201950020")]
        public void GpxParserTest(string xmlFile, string time, string id){

            var context = A.Fake<HttpContext>();  
            var config = A.Fake<IConfiguration>();
            
            var request = context.Request;

            request.Body = File.OpenRead(xmlFile);           
            var protocol = new HermesGpxProtocol(_logger,config);
            protocol.ShouldNotBeNull();

            var data = new Gpx.Core.Contracts.ProtocolContext
            {
                FileByteStream = request.Body,
                Context = $"{time}_{id}.gpx&{1}"
            };
            
            using(var parser = new GpxParser(_logger)){
                parser.Parsed += (sender, arg) =>{
                    arg.Exception.ShouldBeNull();
                    arg.ParserResult.ShouldNotBeNull();
                    arg.ParserResult.Routes.Count().ShouldBeGreaterThan(0);
                };                  
                parser.Parse(data);
            }
        }
        
        [Fact]
        public void DataContextTest()
        {
            using (var context = new HermesDataContext())
            {
                context.ShouldNotBeNull();
                var list = context.TransportTypes.
                    Select(x => x.TypeName).ToList();
                list.ShouldNotBeNull();
                list.Count.ShouldBeEqualTo(2);                
            }            
        }
    }
}