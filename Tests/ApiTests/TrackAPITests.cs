using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using SpotifyTestProject.API.Services;
using SpotifyTestProject.API.Clients;
using SpotifyTestProject.API.Interfaces;
using SpotifyTestProject.Core.Exceptions;

namespace SpotifyTestProject.Tests.ApiTests
{
    [TestFixture]
    public class TrackAPITests
    {
        private IPlaylistService _playlistService = null!;
        private ITrackService _trackService = null!;
        private string _playlistId = null!;
        private const string TRACK_URI = "spotify:track:4iV5W9uYEdYUVa79Axb7Rh"; 

        [SetUp]
        public async Task Setup()
        {
            var httpClient = new HttpClient();
            IAuthService authService = new AuthService(httpClient);
            ISpotifyApiClient apiClient = new SpotifyApiClient(httpClient, authService);
            _playlistService = new PlaylistService(apiClient);
            _trackService = new TrackService(apiClient);
            
           
            _playlistId = await _playlistService.CreatePlaylist(
                "Track Test Playlist " + DateTime.Now.Ticks,
                "Test playlist for track operations",
                false);
        }

        [TearDown]
        public async Task Cleanup()
        {
            try
            {
                if (!string.IsNullOrEmpty(_playlistId))
                    await _playlistService.DeletePlaylist(_playlistId);
            }
            catch { /* Игнорируем ошибки удаления в тестах */ }
        }

        [Test]
        public async Task AddTrackToPlaylist_ValidData_ReturnsSnapshotId()
        {

            var snapshotId = await _trackService.AddTracksToPlaylist(
                _playlistId,
                new List<string> { TRACK_URI },
                0);
            

            Assert.That(snapshotId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void RemoveTrackFromPlaylist_ValidData_SkipTest()
        {

            Assert.Ignore("Skipping test due to API limitations. Manual verification required.");
        }
    }
}



