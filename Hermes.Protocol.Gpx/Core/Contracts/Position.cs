namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public class Position : IGeoPoint
    {
        public Position(double lat, double lon)
        {
            Lat = lat;
            Lng = lon;
        }
        #region IGeoPoint implementation

        public double Lat { get; set; }

        public double Lng { get; set; }

        public bool IsEmpty { get; set; }

        #endregion
    }
}