using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifyTestProject.Core.Exceptions;
using SpotifyTestProject.API.Interfaces;

namespace SpotifyTestProject.API.Clients
{
    public class SpotifyApiClient : ISpotifyApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public SpotifyApiClient(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpClient.BaseAddress = new System.Uri("https://api.spotify.com/v1/");
        }

        public async Task<T> ExecuteAsync<T>(HttpRequestMessage request)
        {
            var accessToken = await _authService.GetAccessTokenAsync();
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"Request failed: {response.StatusCode}", response.StatusCode);
            
            return JsonConvert.DeserializeObject<T>(content)!;
        }
    }
}
