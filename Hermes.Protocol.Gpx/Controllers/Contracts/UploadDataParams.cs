using System;
using System.Xml.Linq;

namespace Hermes.Protocol.Gpx.Controllers.Contracts
{
    internal class UploadDataParams:IUploadDataParams
    {
        public double CarId { get; set; }
        public DateTime Date { get; set; }
        public bool IsFiltered { get; set; }
        public int CarType { get; set; }
        public XDocument UploadData { get; set; }
    }

}