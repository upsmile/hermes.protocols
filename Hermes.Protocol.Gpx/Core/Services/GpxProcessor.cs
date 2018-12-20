using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hermes.Protocol.Gpx.Core.Contracts;
using Serilog;

namespace Hermes.Protocol.Gpx.Core.Services
{
    public sealed class GpxParser : IHermesDataProcessor, IDisposable
    {
        private  readonly ILogger _logger;

        public GpxParser(ILogger logger){
            _logger= logger ?? throw new ArgumentNullException(nameof(logger)) ;
        }

        public event HermesDataProcessorEvent Parsed;
        private void OnCompiled(HermesDataProcessorEventArg arg)
        {
            Parsed?.Invoke(this, arg);
        }
        public void Parse(IUploadData data)
        {
            _logger.Information("begin parse gpx data");
            var track = data ?? throw new ArgumentNullException(nameof(data));
            var argument = new HermesDataProcessorEventArg();
            try
            {
                track.With(x=>x.FileByteStream.Do(stream =>{
                    var document = XDocument.Load(stream);
                    var cfs = data.Context.Split('\\')[data.Context.Split('\\').Length - 1];
                    var param = cfs.ToUploadParameters();
                    param.UploadData = document;
                    var docList = new List<XDocument> { document };
                    var segments = new List<List<ITrackPoint>>();
                    foreach (var d in docList.Select(doc => doc.ToTrackSegments()))
                    {
                        segments.AddRange(d);
                    }                    
                    var routesByDays = segments.ToSegmentsDictionary().ToFilteredRoutes();

                    var routes = routesByDays as TrackRouteStorage[] ?? routesByDays.ToArray();
                    var distancesByDate = routes
                        .SelectMany(d => d.Keys).ToDictionary(key => key.TrackDate
                        , key => key.Distance);
                    var result = new HermesGpxResult
                    {
                        Dates = distancesByDate,
                        Routes = routes,
                        Params = param
                    };
                    argument.Result = result;
                }));                
            }
            catch (Exception exception)
            {
                argument.Exception = exception;
                throw;
            }
            finally
            {
                _logger.Information("end parse gpx data");
                OnCompiled(argument);
            }
        }

        public void Dispose()
        {
           if(Parsed != null) Parsed += null;
        }
    }

}