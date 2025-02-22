using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Domain.Interfaces;

namespace Chords.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;

        public FavoriteService(IUserRepository userRepository, ISongRepository songRepository)
        {
            _userRepository = userRepository;
            _songRepository = songRepository;
        }

        public async Task AddFavoriteAsync(int userId, int songId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var song = await _songRepository.GetByIdAsync(songId);
            if (user == null || song == null)
                throw new Exception("Пользователь или песня не найдены");

            if (user.FavoriteSongs.Any(fs => fs.SongId == songId))
                throw new Exception("Песня уже добавлена в избранное");

            user.FavoriteSongs.Add(new UserSong { UserId = userId, SongId = songId });
            await _userRepository.UpdateAsync(user);
        }

        public async Task RemoveFavoriteAsync(int userId, int songId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.FavoriteSongs == null)
                throw new Exception("Пользователь не найден или избранное отсутствует");

            var favorite = user.FavoriteSongs.FirstOrDefault(fs => fs.SongId == songId);
            if (favorite == null)
                throw new Exception("Песня не найдена в избранном");

            user.FavoriteSongs.Remove(favorite);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<Song>> GetFavoritesAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.FavoriteSongs == null)
                throw new Exception("Пользователь не найден или избранное отсутствует");

            return user.FavoriteSongs.Select(fs => fs.Song);
        }
    }
}
