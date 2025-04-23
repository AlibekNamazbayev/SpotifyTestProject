using System.Threading.Tasks;

namespace SpotifyTestProject.API.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetAccessTokenAsync();
    }
}
