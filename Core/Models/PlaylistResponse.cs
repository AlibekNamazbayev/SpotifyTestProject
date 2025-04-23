using Newtonsoft.Json;

namespace SpotifyTestProject.Core.Models
{
    public class PlaylistResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
        
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
        
        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        
        [JsonProperty("public")]
        public bool Public { get; set; }
    }
}
