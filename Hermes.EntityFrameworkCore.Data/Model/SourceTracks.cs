using System;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class SourceTracks
    {
        public int Id { get; set; }
        public decimal? CarId { get; set; }
        public DateTime? TrackDate { get; set; }
        public byte[] TrackSource { get; set; }
    }
}
