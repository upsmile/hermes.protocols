using System;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    /// <summary>
    /// Определяет точку трека
    /// </summary>
    public interface ITrackPoint
    {
        /// <summary>
        /// высота
        /// </summary>
        double Ele { get; set; }
        /// <summary>
        /// Широта
        /// </summary>        
        double Latitude { get; set; }
        /// <summary>
        /// Долгота
        /// </summary>
        double Longtitude { get; set; }
        /// <summary>
        /// Время замера
        /// </summary>
        DateTime Time { get; set; }
        /// <summary>
        /// Скорость объекта(м/с)
        /// </summary>
        double Speed { get; set; }
        /// <summary>
        /// Наименование точки
        /// </summary>
        string Name { get; set; }

        string Fix { get; set; }
    }
}