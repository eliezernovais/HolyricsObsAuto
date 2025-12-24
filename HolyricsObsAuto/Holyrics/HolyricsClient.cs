using HolyricsObsAuto.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HolyricsObsAuto.Holyrics
{
    public class HolyricsClient
    {
        private readonly HttpClient _httpClient;

        public HolyricsClient()
        {
            _httpClient = new HttpClient();
        }
        private string HolyricsUrl = $"http://{Dados.IPHolyrics}/view/text.json";

        public async Task<string> GetTextoAtualAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(HolyricsUrl);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return json;

        }
        public async Task<HolyricsTextResponse> GetEstadoAtualAsync()
        {
            string json = await GetTextoAtualAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            HolyricsTextResponse data = JsonSerializer.Deserialize<HolyricsTextResponse>(json, options);

            return data;
        }


    }
}
