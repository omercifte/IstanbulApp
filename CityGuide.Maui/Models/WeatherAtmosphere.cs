using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityGuide.Maui.Models
{
    public class WeatherAtmosphere
    {
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }
}
