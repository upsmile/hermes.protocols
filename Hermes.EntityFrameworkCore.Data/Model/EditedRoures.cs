using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class EditedRoures
    {
        public int Id { get; set; }
        public int? ProcessedTrackId { get; set; }
        public string Jsonobject { get; set; }

        public virtual ProcessedTracks ProcessedTrack { get; set; }
    }
}
