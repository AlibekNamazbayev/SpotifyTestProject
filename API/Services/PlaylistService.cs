using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpotifyTestProject.API.Clients;
using SpotifyTestProject.Core.Models;
using SpotifyTestProject.Core.Exceptions;
using SpotifyTestProject.API.Interfaces;

namespace SpotifyTestProject.API.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ISpotifyApiClient _apiClient;

        public PlaylistService(ISpotifyApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> CreatePlaylist(string name, string description, bool isPublic)
        {
            var userId = await GetUserIdAsync();
            var request = new HttpRequestMessage(HttpMethod.Post, $"users/{userId}/playlists")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { name, description, @public = isPublic }),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _apiClient.ExecuteAsync<CreatePlaylistResponse>(request);
            return response.Id;
        }

        public async Task<PlaylistResponse> GetPlaylist(string playlistId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"playlists/{playlistId}");
            return await _apiClient.ExecuteAsync<PlaylistResponse>(request);
        }

        public async Task UpdatePlaylist(string playlistId, string name, string description, bool isPublic)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"playlists/{playlistId}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new { name, description, @public = isPublic }),
                    Encoding.UTF8,
                    "application/json")
            };

            await _apiClient.ExecuteAsync<object>(request);
        }

        public async Task DeletePlaylist(string playlistId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"playlists/{playlistId}/followers");
            await _apiClient.ExecuteAsync<object>(request);
        }

        private async Task<string> GetUserIdAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "me");
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);
            return response.Id;
        }
    }
}
