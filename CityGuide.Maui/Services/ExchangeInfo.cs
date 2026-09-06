
using System.Text.Json.Serialization;


namespace CityGuide.Maui.Services
{
    public class ExchangeInfo
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("rate")]
        public double Rate { get; set; }
    }
}
