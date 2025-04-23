using Newtonsoft.Json;

namespace SpotifyTestProject.Core.Models
{
    public class UserResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
    }
}
