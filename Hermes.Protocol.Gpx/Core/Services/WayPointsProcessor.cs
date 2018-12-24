using System;
using System.Collections.Generic;
using System.Linq;
using Hermes.Protocol.Gpx.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Serilog;
// ReSharper disable All

namespace Hermes.Protocol.Gpx.Core.Services
{
    public sealed class WayPointsProcessor : IDisposable
    {
        public event ProcessorEvent Processed;

        private void OnProcessed(PointsProcessorEntArg arg)
        {
            Processed?.Invoke(this, arg);
        }

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public WayPointsProcessor(ILogger logger, IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void GetReport(IWayReportConfig config)
        {
            var source = config ?? throw new ArgumentNullException(nameof(config));
            var argument = new PointsProcessorEntArg();
            try
            {

                var result = new PointsProcessorResult();                
                var way = new Dictionary<DateTime, IEnumerable<DeliveryPointCache>>();

                var parameters = source.Context.Split('\\')[source.Context.Split('\\').Length - 1].ToUploadParameters();
                var route = source.Track;

                var fleetType = parameters.CarType;
                var fleetId = parameters.CarId;

                var cnf = new WayReportConfig()
                {
                    Track = route,
                    Context = config.Context
                };
                
                var dates = config.Track.Dates;   
                var mapping = config.Track.Routes.ToTrackMapping();
                foreach (var date in dates)
                {
                    using (var cache = new PointsCacheService(_configuration,_logger))
                    {
                        cache.Loaded += (s, o) =>
                        {
                            o.With(c => c.Result.Do(res =>
                            {
                                var points = res.Cast<DeliveryPointCache>().ToList();
                                way.Add(date.Key, points);
                                _logger.Debug($"load points from cache successfully complete " +
                                              $"{date.Key.ToShortDateString()}" +
                                              $"id: {fleetId} type: {fleetType}");
                            }));
                            o.With(ex => ex.Exception.Do(e =>
                            {
                                argument.Exception = e;
                                _logger.Fatal(e, e.Message);
                                throw e;
                            }));
                        };
                            
                        _logger.Debug($"start load points from cache");
                        cache.Get(fleetId,fleetType,date.Key);
                    } 
                }
                var path = new List<IPoint>();                                
                result.Result = new Dictionary<DateTime,IEnumerable<IDeliveryPoint>>();               
                foreach (var date in mapping.Keys)
                {                    
                    var points = way[date];
                    var track = mapping[date];
                    var nearest = track.NearestPoints(points, config.Radius/1000);                    
                    foreach (var segment in track)
                    {
                        path.AddRange(segment);
                    }                    
                    var visited = nearest.ToVisitedPointsList(path, config.Radius/1000);                    
                    result.Result.Add(date,visited);
                    _logger.Debug($"{date.ToShortDateString()} id: {fleetId} type:{fleetType} report successfully complete");
                }
                argument.Result = result;  
                _logger.Debug($"id: {fleetId} type:{fleetType} report successfully complete");
            }
            catch (Exception exception)
            {
                argument.Exception = exception;
                _logger.Fatal(exception, exception.Message);
                throw;
            }
            finally
            {
                OnProcessed(argument);
            }
        }

        public void Dispose()
        {
            Processed += null;
        }
    }
}