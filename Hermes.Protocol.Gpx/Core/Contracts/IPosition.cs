namespace Hermes.Protocol.Gpx.Core.Contracts
{
    /// <summary>
    /// Представление координат на карте
    /// </summary>
    public interface IPosition
    {
        
        double Lat { get; set; }
        
        double Lng { get; set; }
        
        bool IsEmpty { get; set; }
    }
}