using System;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class ReadLog
    {
        public Guid Id { get; set; }
        public int? FleetType { get; set; }
        public decimal? FleetId { get; set; }
        public DateTime? DateEvent { get; set; }
        public int? LodadedData { get; set; }

        public virtual LoadedData LodadedDataNavigation { get; set; }
    }
}
