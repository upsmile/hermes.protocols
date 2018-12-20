using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class UserStatuses
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int TrackId { get; set; }

        public virtual TrackStatuses Status { get; set; }
        public virtual ProcessedTracks Track { get; set; }
    }
}
