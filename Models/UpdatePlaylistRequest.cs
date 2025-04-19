namespace SpotifyAPITests.Models
{
    public class UpdatePlaylistRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Public { get; set; }
    }
}