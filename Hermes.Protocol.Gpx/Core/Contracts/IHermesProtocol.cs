namespace Hermes.Protocol.Gpx.Core.Contracts
{

    public delegate void HermesProtocolEvent(object sender, HermesProtocolEventArg arg);

    public interface IHermesProtocol
    {
        void GetMessage(object data);
        
        event HermesProtocolEvent Posted;
    }
}