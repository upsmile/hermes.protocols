using System;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    /// <summary>
    /// Представление координат на карте
    /// </summary>
    public interface IGeoPoint
    {
        double Lat { get; set; }
        double Lng { get; set; }
        Boolean IsEmpty { get; set; }
    }
}