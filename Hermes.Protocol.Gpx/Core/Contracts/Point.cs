using System;

namespace Hermes.Protocol.Gpx.Core.Contracts
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

        public IPosition Position { get; set; }

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