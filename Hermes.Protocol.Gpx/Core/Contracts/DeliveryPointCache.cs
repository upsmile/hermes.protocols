using Newtonsoft.Json;

namespace Hermes.Protocol.Gpx.Core.Contracts
{
 
   public partial class DeliveryPointCache
    {
        [JsonProperty("Position")]
        public Pos Position { get; set; }

        [JsonProperty("Header")]
        public Header Header { get; set; }

         public static DeliveryPointCache FromJson(string json) => JsonConvert.DeserializeObject<DeliveryPointCache>(json, Converter.Settings);
    }
}