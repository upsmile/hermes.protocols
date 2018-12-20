using System;
using Hermes.Protocol.Gpx.Controllers.Contracts;
using Serilog;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
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

        public void Process()
        {
            var argument = new PointsProcessorEntArg();
            try
            {
                _logger.Information("start compile track file");
                var result = new PointsProcessorResult();
                
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                argument.Exception = exception;
                _logger.Error(exception, exception.Message);
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