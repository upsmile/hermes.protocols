using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public sealed class HermesDataProcessorEventArg : EventArgs
    {
        public IHermesGpxResult Result { get; set; }
        public Exception Exception { get; set; }
    }

    public delegate void HermesDataProcessorEvent(object sender, HermesDataProcessorEventArg arg);

}