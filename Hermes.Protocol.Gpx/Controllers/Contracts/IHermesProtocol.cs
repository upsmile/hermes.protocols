using System.IO;
using Serilog;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    public interface IUploadData
    {
        string Context { get; set; }
        Stream FileByteStream { get; set; }
    }

    public delegate void HermesProtocolEvent(object snder, HermesProtocolEventArg arg);

    public interface IHermesProtocol
    {
        void Post(object data, ILogger logger);
        event HermesProtocolEvent Posted;
    }
}