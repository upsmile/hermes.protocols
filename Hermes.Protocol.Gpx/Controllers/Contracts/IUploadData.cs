using System.IO;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public interface IUploadData
    {
        string Context { get; set; }
        Stream FileByteStream { get; set; }
    }
}