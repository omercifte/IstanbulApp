using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityGuide.Maui.Models
{
    public class CurrentObservation
    {
        [JsonPropertyName("condition")]
        public WeatherCondition Condition { get; set; } = new();

        [JsonPropertyName("atmosphere")]
        public WeatherAtmosphere Atmosphere { get; set; } = new();
    }
}
