using Newtonsoft.Json;

namespace SpotifyAPITests.Models
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }
    }
}
