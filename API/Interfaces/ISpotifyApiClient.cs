using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyTestProject.API.Interfaces
{
    public interface ISpotifyApiClient
    {
        Task<T> ExecuteAsync<T>(HttpRequestMessage request);
    }
}
