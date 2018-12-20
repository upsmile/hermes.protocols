using System;
using Hermes.Protocol.Gpx.Controllers.Contracts;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    /// <summary>
    /// Точка трека
    /// </summary>
    internal class Point : IPoint
    {
        public Point()
        {
            Position = new Position(0, 0);
        }

        #region IRoutePoint implementation

        public IGeoPoint Position { get; set; }

        public DateTime Time { get; set; }

        public double Ele
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public double Speed { get; set; }
        #endregion
    }
}