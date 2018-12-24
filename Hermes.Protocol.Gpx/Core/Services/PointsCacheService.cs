using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Hermes.Protocol.Gpx.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
// ReSharper disable All

namespace Hermes.Protocol.Gpx.Core.Services
{
    public class PointsCacheService : IDisposable
    {

        private IConfiguration _configuration;
        private ILogger _logger;

        public PointsCacheService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(configuration));
        }

        public event PointsLoaderEvent Loaded;


        internal void DoLoad(PointsLoaderEvtArg arg)
        {
            Loaded?.Invoke(this, arg);
        }

        public void Get(double id, int type, DateTime date)
        {
            var argument = new PointsLoaderEvtArg();
            try
            {
                var d = date.Date.ToString(new CultureInfo("ru-Ru"));
                var tid = ((long)id).ToString();
                var pointsApi = _configuration["way"];
                var cacheApi = _configuration["cache"];

                switch (type)
                {
                    case 1:                        
                        pointsApi += $"tr?id={tid}&date={d}";            
                        break;                    
                    case 2:
                        pointsApi += $"ta?id={tid}&date={d}";
                        break;
                    default:
                        throw new NotImplementedException();
                }

                using (var client = new WebClient())
                {
                    var array = client.DownloadData(pointsApi);
                    var json = Encoding.UTF8.GetString(array);
                    var pointsList = JsonConvert.DeserializeObject<List<double>>(json);
                    if (pointsList.Count == 0)
                    {
                        argument.Result = pointsList.Cast<object>().ToList();
                        _logger.Warning($"запланированных точек для посещения {d} транспортным средством {id} не обнаружено");
                    }
                    else
                    {
                        _logger.Information($"для траспортного средства {id} на дату {d} обнаружено" +
                            $" {pointsList.Count} запланированных точек посещения");
                        var result = new List<object>();
                        foreach (var t in pointsList)
                        {
                            try
                            {
                                var url = $"{cacheApi}{t}";
                                array = client.DownloadData(url);
                                json = Encoding.UTF8.GetString(array);
                                var point = DeliveryPointCache.FromJson(json);
                                result.Add(point);
                            }
                            catch (Exception e)
                            {
                                _logger.Warning(e, $"возникла ошибка при получении данных точки {t}{ e.Message}");
                            }
                        }
                        argument.Result = result;
                    }
                }
            }
            catch (System.Exception exception)
            {
                argument.Exception = exception;
                _logger.Error(exception, exception.Message);
                throw;
            }
            finally
            {
                DoLoad(argument);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Loaded += null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PointsCacheService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}