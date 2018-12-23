using System;
using Hermes.Protocol.Gpx.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Serilog;

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
                foreach (var date in dates)
                {
                    // загрузка данных  о маршруте траспорта
                    /*   object wayPoints = null;                       
                    using (var cache = new PointsCacheService(_configuration,_logger))
                    {
                        cache.Loaded += (s,o) =>{
                            o.With(c=> c.Result.Do(rslt => {
                                wayPoints = rslt;  
                            }));

                            o.With(ex => ex.Exception.Do(e => {
                                argument.Exception = e;
                                _logger.Fatal(e, e.Message);
                                throw e;
                            }));
                        };
                        cache.Get(fleetId,fleetType,parameters.Date);
                    } */
                }
                // получение результата посещенных точек траспортных средств
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