using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyTestProject.Core.Models;

namespace SpotifyTestProject.API.Interfaces
{
    public interface ITrackService
    {
        Task<string> AddTracksToPlaylist(string playlistId, List<string> trackUris, int? position = null);
        Task<string> RemoveTracksFromPlaylist(string playlistId, List<string> trackUris, string? snapshotId = null);
    }
}
