using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class ProcessedTracks
    {
        public ProcessedTracks()
        {
            EditInfo = new HashSet<EditInfo>();
            EditedRoures = new HashSet<EditedRoures>();
            TrackContent = new HashSet<TrackContent>();
            UserStatuses = new HashSet<UserStatuses>();
        }

        public int Id { get; set; }
        public decimal CarId { get; set; }
        public DateTime DateTrack { get; set; }
        public double Dictance { get; set; }
        public TimeSpan Duration { get; set; }
        public int? TransportType { get; set; }

        public virtual ICollection<EditInfo> EditInfo { get; set; }
        public virtual ICollection<EditedRoures> EditedRoures { get; set; }
        public virtual ICollection<TrackContent> TrackContent { get; set; }
        public virtual ICollection<UserStatuses> UserStatuses { get; set; }
    }
}
