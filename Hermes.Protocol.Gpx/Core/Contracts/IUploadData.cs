using System.IO;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public interface IUploadData
    {
        string Context { get; set; }
        Stream FileByteStream { get; set; }
    }
}