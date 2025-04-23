using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifyTestProject.Core.Utils;
using SpotifyTestProject.Core.Models;
using SpotifyTestProject.API.Interfaces;
using System;
using System.Collections.Generic;

namespace SpotifyTestProject.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken = null!;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var clientId = EnvironmentHelper.GetRequiredVariable("SPOTIFY_CLIENT_ID");
            var clientSecret = EnvironmentHelper.GetRequiredVariable("SPOTIFY_CLIENT_SECRET");
            var refreshToken = EnvironmentHelper.GetRequiredVariable("SPOTIFY_REFRESH_TOKEN");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(content)!;
            
            _accessToken = authResponse.AccessToken;
            return _accessToken;
        }
    }
}
