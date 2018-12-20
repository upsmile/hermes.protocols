using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <inheritdoc />
    internal sealed class PointsProcessorResult: IPointsProcessorResult
    {
        /// <inheritdoc />
        public Dictionary<DateTime, IEnumerable<IDeliveryPoint>> Result { get; set; }
    }
}