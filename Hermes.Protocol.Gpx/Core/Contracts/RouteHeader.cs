using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public sealed class RouteHeader
    {
        /// <summary>
        /// Дистанция
        /// </summary>
        public double Distance { get; set; }
        
        /// <summary>
        /// Длительность
        /// </summary>
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// Дата трека
        /// </summary>
        public DateTime TrackDate { get; set; }
        
        /// <summary>
        /// Начало движения
        /// </summary>
        public DateTime Start { get; set; }
        
        /// <summary>
        /// Окончание движения
        /// </summary>
        public DateTime Stop { get; set; }
        
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Guid { get; set; }
    }
}