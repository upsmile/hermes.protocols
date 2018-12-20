using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public sealed class HermesProtocolEventArg : EventArgs
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}