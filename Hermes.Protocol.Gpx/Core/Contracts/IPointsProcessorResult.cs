using System;
using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IPointsProcessorResult
    {
        /// <summary>
        /// Точки посещения
        /// </summary>
        Dictionary<DateTime, IEnumerable<IDeliveryPoint>> Result { get; set; }
    }
}