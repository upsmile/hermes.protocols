using Newtonsoft.Json;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
    public class Header
    {
        [JsonProperty("Code")]
        public long Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Branch")]
        public Branch Branch { get; set; }

        [JsonProperty("RequestString")]
        public string RequestString { get; set; }

        [JsonProperty("Contractors")]
        public string Contractors { get; set; }

        [JsonProperty("ResponceString")]
        public object ResponceString { get; set; }
    }
}