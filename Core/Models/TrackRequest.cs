using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpotifyTestProject.Core.Models
{
    public class TrackRequest
    {
        [JsonProperty("uris")]
        public List<string> Uris { get; set; } = new List<string>();

        [JsonProperty("position")]
        public int? Position { get; set; }
    }

    public class RemoveTrackRequest
    {
        [JsonProperty("tracks")]
        public List<TrackReference> Tracks { get; set; } = new List<TrackReference>();

        [JsonProperty("snapshot_id")]
        public string? SnapshotId { get; set; }
    }

    public class TrackReference
    {
        [JsonProperty("uri")]
        public string Uri { get; set; } = null!;
    }

    public class TrackResponseSnapshot
    {
        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; } = null!;
    }
}
