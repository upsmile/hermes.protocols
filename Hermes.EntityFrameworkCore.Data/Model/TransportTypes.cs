using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class TransportTypes
    {
        public TransportTypes()
        {
            EventVisits = new HashSet<EventVisits>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<EventVisits> EventVisits { get; set; }
    }
}
