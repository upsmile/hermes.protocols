using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class TrackContent
    {
        public int Id { get; set; }
        public int? ProcessedTrack { get; set; }
        public string LoadedData { get; set; }

        public virtual ProcessedTracks ProcessedTrackNavigation { get; set; }
    }
}
