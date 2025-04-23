using System.Threading.Tasks;
using SpotifyTestProject.Core.Models;

namespace SpotifyTestProject.API.Interfaces
{
    public interface IPlaylistService
    {
        Task<string> CreatePlaylist(string name, string description, bool isPublic);
        Task<PlaylistResponse> GetPlaylist(string playlistId);
        Task UpdatePlaylist(string playlistId, string name, string description, bool isPublic);
        Task DeletePlaylist(string playlistId);
    }
}
