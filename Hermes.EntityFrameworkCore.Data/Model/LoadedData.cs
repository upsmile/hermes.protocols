using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class LoadedData
    {
        public LoadedData()
        {
            ReadLog = new HashSet<ReadLog>();
        }

        public int Id { get; set; }
        public DateTime DateEvent { get; set; }
        public DateTime DateFirst { get; set; }
        public DateTime DateLast { get; set; }
        public Guid ReplId { get; set; }
        public byte[] TrackData { get; set; }
        public decimal? CarId { get; set; }

        public virtual ICollection<ReadLog> ReadLog { get; set; }
    }
}
