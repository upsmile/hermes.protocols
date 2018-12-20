using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public sealed class PointsProcessorEntArg : EventArgs
    {
        public   IPointsProcessorResult Result { get; set; }
        
        public Exception Exception { get; set; }
    }

    public delegate void ProcessorEvent(object sender, PointsProcessorEntArg arg);
}