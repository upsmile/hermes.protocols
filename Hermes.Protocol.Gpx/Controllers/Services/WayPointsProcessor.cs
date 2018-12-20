using System;
using System.Collections.Generic;
using System.Linq;
using Hermes.Protocol.Gpx.Controllers.Contracts;
using Serilog;

namespace Hermes.Protocol.Gpx.Controllers.Services
{


    public static class WayPointsProcessorExtention{

        private static IEnumerable<IPoint> ToPoints(this IEnumerable<ITrackPoint> points)
        {
            return points.Select(item => item.ToPoint()).ToList();
        }

        private static IPoint ToPoint(this ITrackPoint tp)
        {
            var result = new Point
                             {
                                 Position = new Position(tp.Latitude, tp.Longtitude),
                                 Speed = tp.Speed,
                                 Name = tp.Name,
                                 Time = tp.Time,
                                 Ele = tp.Ele
                             };
            return result;
        }

        /// <summary>
        /// Преобразование данных для дальнейшей обработки
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> ToRouteData(this IEnumerable<TrackRouteStorage> context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var result = new Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>>();
            foreach (var item in context)
            {
                foreach (var d in item)
                {
                    var trackDate = d.Key.TrackDate;
                    var segments = d.Value;
                    var rlist = segments.Select(sgmnt => sgmnt.Value.ToPoints()).ToList();
                    result.Add(trackDate, rlist);
                }
            }
            return result;
        }
    }

    public sealed class WayPointsProcessor :IDisposable
    {
        public event ProcessorEvent Processed;

        private void OnProcessed(PointsProcessorEntArg arg)
        {
            Processed?.Invoke(this, arg);
        }

        private readonly ILogger _logger;
        
        public WayPointsProcessor(ILogger logger)
        {
            _logger = logger;            
        }

        public void Process(IDeliveryPointsPorcessorConfig config)
        {
            var track = config ?? throw new ArgumentNullException(nameof(config));
            var argument = new PointsProcessorEntArg();
            try
            {
                _logger.Information("begin create visited points report");
                var result = new PointsProcessorResult();  
                var parameters = track.Context.Split('\\')[track.Context.Split('\\').Length - 1].ToUploadParameters();
                var route = track.Routes
                                .Cast<TrackRouteStorage>()
                                .ToRouteData(); 

                var fleetType = parameters.CarType;
                var fleetId = parameters.CarId;
                var dates = route.Keys.ToList();
                var dpconfig = new DeliveryPointProcessorConfig{
                    Dates = dates,
                    TransportId = fleetId,
                    Routes = route
                    };                              
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                argument.Exception = exception;
                _logger.Fatal(exception, exception.Message);
                throw;
            }
            finally
            {
                _logger.Information("compile track file finished");
                OnProcessed(argument);
            }
        }
    
        public void Dispose()
        {
            Processed += null;
        }
    }
}