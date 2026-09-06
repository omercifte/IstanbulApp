using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CityGuide.Maui.Models;

namespace CityGuide.Maui.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        private const string BaseUrl = "https://localhost:7112";

        public EventApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<SpecialEvent>>GetEventsAsync()
        {
            var events = await _httpClient.GetFromJsonAsync<List<SpecialEvent>>($"{BaseUrl}/api/events");
            return events ?? new List<SpecialEvent>();
        }
    }
}
