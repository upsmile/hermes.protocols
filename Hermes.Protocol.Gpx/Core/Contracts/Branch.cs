using System;
using Newtonsoft.Json;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public class Branch
    {
        [JsonProperty("Name")]
        public object Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }
    }
}