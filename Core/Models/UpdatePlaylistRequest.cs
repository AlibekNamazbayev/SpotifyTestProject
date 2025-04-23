namespace SpotifyTestProject.Core.Models
{
    public class UpdatePlaylistRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Public { get; set; }
    }
}
