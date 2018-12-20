using System;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public sealed class PointsProcessorEntArg : EventArgs
    {
        public   IPointsProcessorResult Result { get; set; }
        
        public Exception Exception { get; set; }
    }
}