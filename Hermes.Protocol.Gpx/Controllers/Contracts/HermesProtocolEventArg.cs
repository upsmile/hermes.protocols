using System;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    public sealed class HermesProtocolEventArg : EventArgs
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}