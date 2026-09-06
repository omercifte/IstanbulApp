using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CityGuide.Maui.Models;

namespace CityGuide.Maui.Services
{
    public class WeatherApiService
    {
        private const string ApiKey = "da0ec2c8c0msh4a5c6cf4b356cd9p1337c7jsnfb4eb0eac6b6";
        private const string ApiHost = "yahoo-weather5.p.rapidapi.com";

        public async Task<WeatherResponse?> GetMilanoWeatherAsync()
        {
            using var client = new HttpClient();

            string url = $"https://{ApiHost}/weather?location=milano&format=json&u=c";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", ApiKey },
                    { "x-rapidapi-host", ApiHost },
                },
            };

            try
            {
                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<WeatherResponse>(body);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
