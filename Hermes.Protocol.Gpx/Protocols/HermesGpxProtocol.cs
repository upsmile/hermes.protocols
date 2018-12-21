using System;
using System.Diagnostics.CodeAnalysis;
using Hermes.Protocol.Gpx.Core.Contracts;
using Hermes.Protocol.Gpx.Core.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Hermes.Protocol.Gpx.Protocols
{

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public sealed class HermesGpxProtocol : IHermesProtocol, IDisposable
    {
        
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        
        public HermesGpxProtocol(ILogger logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public event HermesProtocolEvent Posted;

        private void OnPosted(HermesProtocolEventArg arg)
        {
            Posted?.Invoke(this, arg);
        }

        public void GetMessage(object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.GetType() != typeof(ProtocolContext)) throw new InvalidCastException();
            var argument = new HermesProtocolEventArg();
            try
            {
                var body = (IProtocolContext)data;

                using (var parser = new GpxParser(_logger))
                {
                    parser.Parsed += (sender, arg) =>
                    {
                        arg.With(x => x.Exception.Do(exception =>
                        {
                            argument.Exception = exception;
                            throw exception;
                        }));

                        arg.With(x => x.ParserResult.Do(track =>
                        {
                            _logger.Information("parse gpx file data successfully completed");

                            using (var processor = new WayPointsProcessor(_logger, _configuration))
                            {
                                _logger.Information("begin create visited points report");
                                processor.Processed += (s, a) =>
                                {
                                    if (a.Exception != null)
                                    {
                                        throw a.Exception;
                                    }
                                    if (a.Result != null)
                                    {
                                        throw new NotImplementedException();
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException
                                            ("результат обработки посещенных точек не существует");
                                    }
                                };
                                //Dictionary<DateTime, IEnumerable<IEnumerable<IPoint>>> Routes { get; set; }
                                var config = new WayReportConfig()
                                {
                                    Context = body.Context,
                                    Track = track
                                };
                                
                                processor.GetReport(config);
                            }
                        }));
                    };
                    parser.Parse(body);
                }
            }
            catch (Exception exception)
            {
                argument.Exception = exception;
                _logger.Error(exception, exception.Message);
                throw;
            }
            finally
            {
                OnPosted(argument);
            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Posted += null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HermesGpxProtocol() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}