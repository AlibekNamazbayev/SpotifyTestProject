using Newtonsoft.Json;

namespace SpotifyTestProject.Core.Models
{
    public class CreatePlaylistResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
