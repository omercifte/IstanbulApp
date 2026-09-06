using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityGuide.Maui.Services
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("query")]
        public ExchangeQuery Query { get; set; } = new();

        [JsonPropertyName("info")]
        public ExchangeInfo Info { get; set; } = new();

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public double Result { get; set; }
    }


}
