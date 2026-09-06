using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityGuide.Maui.Services
{
    public class CurrencyApiService
    {
        private const string ApiKey = "da0ec2c8c0msh4a5c6cf4b356cd9p1337c7jsnfb4eb0eac6b6";
        private const string ApiHost = "currency-conversion-and-exchange-rates.p.rapidapi.com";

        public async Task<ExchangeRateResponse?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            using var client = new HttpClient();

            string url = $"https://{ApiHost}/convert?from={fromCurrency}&to={toCurrency}&amount=1";

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
                var result = JsonSerializer.Deserialize<ExchangeRateResponse>(body);

                return result;
            }
            catch (Exception)
            {
                // Ağ hatası, API limiti aşımı, vb. — null dön, çağıran taraf ele alır
                return null;
            }
        }

    }
}
