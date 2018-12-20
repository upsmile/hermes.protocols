using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый авбстрактный класс работы с хранилищем сегментов треков 
    /// </summary>
    public abstract class SegmentBase : Dictionary<RouteHeader, IEnumerable<ITrackPoint>>
    {

    }
}