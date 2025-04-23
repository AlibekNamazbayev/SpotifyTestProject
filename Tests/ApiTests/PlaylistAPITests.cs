using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using SpotifyTestProject.API.Services;
using SpotifyTestProject.API.Clients;
using SpotifyTestProject.API.Interfaces;
using SpotifyTestProject.Core.Models;

namespace SpotifyTestProject.Tests.ApiTests
{
    [TestFixture]
    public class PlaylistAPITests
    {
        private IPlaylistService _playlistService = null!;
        private string _createdPlaylistId = null!;

        [SetUp]
        public async Task Setup()
        {
            var httpClient = new HttpClient();
            IAuthService authService = new AuthService(httpClient);
            ISpotifyApiClient apiClient = new SpotifyApiClient(httpClient, authService);
            _playlistService = new PlaylistService(apiClient);
            

            _createdPlaylistId = await _playlistService.CreatePlaylist(
                "Test Playlist " + DateTime.Now.Ticks,
                "Test Description",
                false);
        }

        [TearDown]
        public async Task Cleanup()
        {
            try
            {
                if (!string.IsNullOrEmpty(_createdPlaylistId))
                    await _playlistService.DeletePlaylist(_createdPlaylistId);
            }
            catch { /* Игнорируем ошибки удаления в тестах */ }
        }

        [Test]
        public void CreatePlaylist_ValidData_ReturnsId()
        {
            Assert.That(_createdPlaylistId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task GetPlaylist_ExistingId_ReturnsPlaylist()
        {
            var playlist = await _playlistService.GetPlaylist(_createdPlaylistId);
            Assert.That(playlist, Is.Not.Null);
            Assert.That(playlist.Id, Is.EqualTo(_createdPlaylistId));
        }

        [Test]
        public async Task UpdatePlaylist_ValidData_UpdatesSuccessfully()
        {
      
            await _playlistService.UpdatePlaylist(
                _createdPlaylistId,
                "Updated Name",
                "Updated Description",
                true);
                

            Assert.Pass("Playlist updated successfully");
        }

        [Test]
        public async Task DeletePlaylist_ExistingId_DeletesSuccessfully()
        {
            await _playlistService.DeletePlaylist(_createdPlaylistId);
            _createdPlaylistId = null!; 
            Assert.Pass("Playlist deleted successfully");
        }
    }
}



