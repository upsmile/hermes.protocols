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
        [InlineData("./Data/131544183883686029_201950020.gpx")]
        public void PostControllerIntegrationTest(string xmlFile)
        {
            var context = A.Fake<HttpContext>();                          
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"cache", "http://93.157.14.7:8089/api/points/"},
                    {"way", "http://93.157.14.7:8086/Hermes.Way.Api/api/"}               
                });

            var config =  configurationBuilder.Build();            
            var request = context.Request;
            request.Body = File.OpenRead(xmlFile);           
            var protocol = new HermesGpxProtocol(_logger, config);
            protocol.ShouldNotBeNull();

            var data = new Gpx.Core.Contracts.ProtocolContext
            {
                FileByteStream = request.Body,
                Context = $"{131900760000000000}_{82131026}.gpx&{2}"
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