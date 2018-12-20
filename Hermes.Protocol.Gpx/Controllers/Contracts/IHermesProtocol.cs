namespace Hermes.Protocol.Gpx.Controllers.Contracts
{

    public delegate void HermesProtocolEvent(object sender, HermesProtocolEventArg arg);

    public interface IHermesProtocol
    {
        void Post(object data);
        
        event HermesProtocolEvent Posted;
    }
}