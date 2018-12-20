using System.Collections.Generic;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    /// <inheritdoc />
    /// <summary>
    /// Базовый  класс работы с хранилищем треков 
    /// </summary>
    public abstract class TrackRouteStorageBase : Dictionary<RouteHeader, SegmentRouteStorageBase>
    {
    }
}