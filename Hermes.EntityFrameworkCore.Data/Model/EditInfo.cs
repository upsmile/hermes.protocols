using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class EditInfo
    {
        public int Id { get; set; }
        public int? ProcessedId { get; set; }
        public string Editinfo1 { get; set; }
        public string Smoothinfo { get; set; }
        public DateTime? Edittingdate { get; set; }
        public decimal? Editor { get; set; }

        public virtual ProcessedTracks Processed { get; set; }
    }
}
