namespace Chords.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }  // Имя пользователя
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Связь «многие-ко-многим» (например, избранные песни)
        public ICollection<UserSong> FavoriteSongs { get; set; } = new List<UserSong>();
    }
}
