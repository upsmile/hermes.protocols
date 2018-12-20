using System;
using System.Collections.Generic;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class Sysdiagrams
    {
        public string Name { get; set; }
        public int PrincipalId { get; set; }
        public int DiagramId { get; set; }
        public int? Version { get; set; }
        public byte[] Definition { get; set; }
    }
}
