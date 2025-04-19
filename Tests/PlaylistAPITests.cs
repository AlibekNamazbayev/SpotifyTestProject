using System;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using SpotifyAPITests.Models;
using SpotifyAPITests.Utils;

namespace SpotifyAPITests.Tests
{
    [TestFixture]
    public class PlaylistAPITests
    {
        private ApiClient _apiClient = null!;
        private string _userId = null!;
        private string _testTrackUri = "spotify:track:4iV5W9uYEdYUVa79Axb7Rh"; //sample track

        [SetUp]
        public void Setup()
        {
            string clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") ?? 
                throw new InvalidOperationException("SPOTIFY_CLIENT_ID not set");
            string clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET") ?? 
                throw new InvalidOperationException("SPOTIFY_CLIENT_SECRET not set");
            string refreshToken = Environment.GetEnvironmentVariable("SPOTIFY_REFRESH_TOKEN") ?? 
                throw new InvalidOperationException("SPOTIFY_REFRESH_TOKEN not set");

            _apiClient = new ApiClient("https://api.spotify.com/v1");
            _apiClient.Authenticate(clientId, clientSecret, refreshToken);
            _userId = GetUserId();
        }

        // Вспомогательные методы
        private string GetUserId()
        {
            var response = _apiClient.Execute(new RestRequest("me"));
            return JsonConvert.DeserializeObject<UserResponse>(response.Content!)?.Id ?? 
                throw new Exception("User ID not found");
        }

        private string CreateTestPlaylist(string? name = null)
        {
            var request = new RestRequest($"users/{_userId}/playlists", Method.Post)
                .AddJsonBody(new CreatePlaylistRequest
                {
                    Name = name ?? "Test Playlist " + DateTime.Now.Ticks,
                    Public = false
                });

            var response = _apiClient.Execute(request);
            return JsonConvert.DeserializeObject<CreatePlaylistResponse>(response.Content!)?.Id ?? 
                throw new Exception("Playlist ID not found");
        }

        private string AddTestTrack(string playlistId, string? trackUri = null)
        {
            var request = new RestRequest($"playlists/{playlistId}/tracks", Method.Post)
                .AddJsonBody(new { uris = new[] { trackUri ?? _testTrackUri } });

            var response = _apiClient.Execute(request);
            var snapshotId = JsonConvert.DeserializeObject<dynamic>(response.Content!)?.snapshot_id;
            return snapshotId?.ToString() ?? throw new Exception("Snapshot ID not found");
        }

        // API 1: Create Playlist
        [Test]
        public void CreatePlaylist_ValidRequest_Returns201()
        {
            var requestBody = new CreatePlaylistRequest
            {
                Name = "New Playlist " + DateTime.Now.Ticks,
                Description = "Test Description",
                Public = false
            };

            var response = _apiClient.Execute(
                new RestRequest($"users/{_userId}/playlists", Method.Post)
                .AddJsonBody(requestBody));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var responseData = JsonConvert.DeserializeObject<CreatePlaylistResponse>(response.Content!);
            Assert.That(responseData?.Name, Is.EqualTo(requestBody.Name));
        }

        // API 2: Edit Playlist
        [Test]
        public void EditPlaylist_ValidRequest_Returns200()
        {
            var playlistId = CreateTestPlaylist();
            
            var updateRequest = new 
            {
                Name = "Updated Name",
                Description = "Updated Description",
                Public = true
            };

            var response = _apiClient.Execute(
                new RestRequest($"playlists/{playlistId}", Method.Put)
                .AddJsonBody(updateRequest));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        // API 3: Add Items
        [Test]
        public void AddItemsToPlaylist_ValidRequest_Returns201()
        {
            var playlistId = CreateTestPlaylist();
            
            var response = _apiClient.Execute(
                new RestRequest($"playlists/{playlistId}/tracks", Method.Post)
                .AddJsonBody(new { uris = new[] { _testTrackUri }, position = 0 }));

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Content?.Contains("snapshot_id"), Is.True);
        }

        // API 4: Remove Items
        [Test]
        public void RemoveItemsFromPlaylist_ValidRequest_Returns200()
        {
            // Arrange
            var playlistId = CreateTestPlaylist();
            var snapshotId = AddTestTrack(playlistId);

            // Act
            var response = _apiClient.Execute(
                new RestRequest($"playlists/{playlistId}/tracks", Method.Delete)
                .AddJsonBody(new { 
                    tracks = new[] { new { uri = _testTrackUri } },
                    snapshot_id = snapshotId 
                }));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content?.Contains("snapshot_id"), Is.True);
        }

        private class UserResponse
        {
            [JsonProperty("id")]
            public string? Id { get; set; }
        }
    }
}