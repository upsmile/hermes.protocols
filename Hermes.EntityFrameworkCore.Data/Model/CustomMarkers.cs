using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class CustomMarkers
    {
        public int Id { get; set; }
        public string ImageUri { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
        public string Description { get; set; }
    }
}
