using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class TrackStatuses
    {
        public TrackStatuses()
        {
            UserStatuses = new HashSet<UserStatuses>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<UserStatuses> UserStatuses { get; set; }
    }
}
