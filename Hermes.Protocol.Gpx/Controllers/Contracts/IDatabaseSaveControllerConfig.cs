namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public interface IDatabaseSaveControllerConfig
    {
        string ConnectionString { get; set; }
        object Request { get; set; }
    }
}