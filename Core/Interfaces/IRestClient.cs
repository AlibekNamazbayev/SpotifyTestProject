using System.Threading.Tasks;
using System.Net.Http;

namespace SpotifyTestProject.Core.Interfaces
{
    public interface IRestClient
    {
        Task<T> ExecuteAsync<T>(HttpMethod method, string endpoint, object? body = null, string? bearerToken = null);
    }
}
