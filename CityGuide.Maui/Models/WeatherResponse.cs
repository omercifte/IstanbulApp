using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityGuide.Maui.Models
{
   
        public class WeatherResponse
        {
            [JsonPropertyName("location")]
            public WeatherLocation Location { get; set; } = new();

            [JsonPropertyName("current_observation")]
            public CurrentObservation CurrentObservation { get; set; } = new();
        }

    }

