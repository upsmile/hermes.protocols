using System;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    internal sealed class TrackPoint: ITrackPoint
    {
        public double Ele { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public DateTime Time { get; set; }
        public double Speed { get; set; }
        public string Name { get; set; }
        public string Fix { get; set; }
    }
}