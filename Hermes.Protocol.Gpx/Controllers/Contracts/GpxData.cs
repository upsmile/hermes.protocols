using System.IO;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    public class GpxData : IUploadData
    {
        public string Context { get ; set; }
        public Stream FileByteStream { get ;set; }
    }
}