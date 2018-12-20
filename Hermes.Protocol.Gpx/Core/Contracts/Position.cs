namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public class Position : IPosition
    {
        public Position(double lat, double lon)
        {
            Lat = lat;
            Lng = lon;
        }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public bool IsEmpty { get; set; }

    }
}