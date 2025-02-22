namespace Chords.Domain.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string LyricsWithChords { get; set; }  // Текст песни с аккордами

        // Обратная связь с пользователями (если требуется)
        public ICollection<UserSong> UserSongs { get; set; } = new List<UserSong>();
    }
}
