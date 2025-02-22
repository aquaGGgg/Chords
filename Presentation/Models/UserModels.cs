namespace Chords.Presentation.Models
{
    public class UpdateUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class FavoriteRequest
    {
        public int SongId { get; set; }
    }
}
