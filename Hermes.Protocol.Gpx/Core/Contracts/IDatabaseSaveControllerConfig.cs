namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IDatabaseSaveControllerConfig
    {
        string ConnectionString { get; set; }
        object Request { get; set; }
    }
}