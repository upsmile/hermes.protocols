using System;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public sealed class HermesDataProcessorEventArg : EventArgs
    {
        public IHermesGpxResult Result { get; set; }
        public Exception Exception { get; set; }
    }
}