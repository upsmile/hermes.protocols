using System;
using Serilog;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    public sealed class HermesGpxProtocol : IHermesProtocol
    {
        public event HermesProtocolEvent Posted;

        private void OnPosted(HermesProtocolEventArg arg)
        {
            Posted?.Invoke(this, arg);
        }

        public void Post(object data, ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.GetType() != typeof(IUploadData)) throw new InvalidCastException();
            var argument = new HermesProtocolEventArg();
            try
            {
                var body = (IUploadData)data;
                throw new NotImplementedException();
            }
            catch (System.Exception exception) 
            {
                argument.Exception = exception;
                logger.Fatal(exception, exception.Message);
                throw;
            }
            finally
            {
                OnPosted(argument);
            }

        }
    }
}