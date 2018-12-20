using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class SourceData
    {
        public int Id { get; set; }
        public int? LoadedDataId { get; set; }
        public byte[] Gpx { get; set; }
        public string FiterDataKey { get; set; }
    }
}
