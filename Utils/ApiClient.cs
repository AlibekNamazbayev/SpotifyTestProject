using RestSharp;
using System;
using System.Text;
using Newtonsoft.Json;
using SpotifyAPITests.Models;

namespace SpotifyAPITests.Utils
{
    public class ApiClient
    {
        private readonly RestClient _client;
        private string? _accessToken;


    public ApiClient(string baseUrl)
    {
        var options = new RestClientOptions(baseUrl);
        _client = new RestClient(options);
    }

        public void Authenticate(string clientId, string clientSecret, string refreshToken)
        {
            try
            {
                var authClient = new RestClient("https://accounts.spotify.com");
                var request = new RestRequest("api/token", Method.Post);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", refreshToken);

                var response = authClient.Execute(request);  // Выполняем запрос

                Console.WriteLine($"Raw response: {response.Content}");

                if (response.Content == null)
                    throw new Exception("Response content is null");

                var data = JsonConvert.DeserializeObject<AuthResponse>(response.Content!);  // Десериализуем вручную
                if (data == null)
                    throw new Exception("Failed to deserialize auth response");

                if (string.IsNullOrEmpty(data.AccessToken))
                    throw new Exception("Access token was null in response");

                _accessToken = data.AccessToken;
                Console.WriteLine($"Access token: {_accessToken?.Substring(0, 10)}...");  // Выводим только первые 10 символов токена
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                throw;
            }
        }

        public RestResponse Execute(RestRequest request)
        {
            if (string.IsNullOrEmpty(_accessToken))
                throw new Exception("No access token available");

            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            request.AddHeader("Content-Type", "application/json");

            Console.WriteLine($"Making request to: {_client.BuildUri(request)}");
            var response = _client.Execute(request);

            Console.WriteLine($"Response: {response.StatusCode} - {response.Content}");
            return response;
        }
    }
}
