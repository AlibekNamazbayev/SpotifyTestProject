using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpotifyTestProject.API.Interfaces;
using SpotifyTestProject.Core.Models;
using Newtonsoft.Json;

namespace SpotifyTestProject.API.Services
{
    public class TrackService : ITrackService
    {
        private readonly ISpotifyApiClient _apiClient;

        public TrackService(ISpotifyApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> AddTracksToPlaylist(string playlistId, List<string> trackUris, int? position = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"playlists/{playlistId}/tracks")
            {
                Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(new { uris = trackUris, position = position }),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _apiClient.ExecuteAsync<TrackResponseSnapshot>(request);
            return response.SnapshotId;
        }

        public async Task<string> RemoveTracksFromPlaylist(string playlistId, List<string> trackUris, string? snapshotId = null)
        {

            var tracks = new List<object>();
            foreach (var uri in trackUris)
            {
                tracks.Add(new { uri });
            }


            var payload = new Dictionary<string, object>
            {
                { "tracks", tracks }
            };

 
            if (!string.IsNullOrEmpty(snapshotId))
            {
                payload.Add("snapshot_id", snapshotId);
            }

            
            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"playlists/{playlistId}/tracks")
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            var response = await _apiClient.ExecuteAsync<TrackResponseSnapshot>(request);
            return response.SnapshotId;
        }
    }
}




