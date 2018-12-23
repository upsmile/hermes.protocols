using Newtonsoft.Json;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public class Pos
    {
        [JsonProperty("Lat")]
        public double Lat { get; set; }

        [JsonProperty("Lon")]
        public double Lon { get; set; }

        [JsonProperty("Ele")]
        public long Ele { get; set; }
    }
}