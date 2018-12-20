using System;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class EventVisits
    {
        public int Id { get; set; }
        public int? FleetType { get; set; }
        public decimal? FleetId { get; set; }
        public DateTime? DateEvent { get; set; }
        public string Points { get; set; }
        public string Exceptedpoints { get; set; }

        public virtual TransportTypes FleetTypeNavigation { get; set; }
    }
}
