using System.IO;

namespace Hermes.Protocol.Gpx.Controllers.Services
{
    internal class GpxData : IUploadData
    {
        public string Context { get ; set; }
        public Stream FileByteStream { get ;set; }
    }
}