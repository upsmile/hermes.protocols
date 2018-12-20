using System;
using System.Linq;
using Hermes.Protocol.Gpx.Core.Contracts;
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

        public WayPointsProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void GetReport(IWayReportConfig config)
        {
            var source = config ?? throw new ArgumentNullException(nameof(config));
            var argument = new PointsProcessorEntArg();
            try
            {
                _logger.Information("begin create visited points report");
                var result = new PointsProcessorResult();
                var parameters = source.Context.Split('\\')[source.Context.Split('\\').Length - 1].ToUploadParameters();
                var route = source.Routes
                                .Cast<Track>()
                                .ToTrackMapping();

                var fleetType = parameters.CarType;
                var fleetId = parameters.CarId;
                var dates = route.Keys.ToList();
                var cnf = new WayReportConfig()
                {
                    Dates = dates,
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