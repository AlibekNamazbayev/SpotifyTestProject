using Newtonsoft.Json;

namespace SpotifyAPITests.Models
{
    public class CreatePlaylistResponse
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }
    }
}
