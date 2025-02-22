using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Domain.Interfaces;

namespace Chords.Application.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _songRepository.GetAllAsync();
        }

        public async Task<Song> GetSongByIdAsync(int id)
        {
            return await _songRepository.GetByIdAsync(id);
        }

        public async Task CreateSongAsync(Song song)
        {
            await _songRepository.AddAsync(song);
        }

        public async Task UpdateSongAsync(Song song)
        {
            await _songRepository.UpdateAsync(song);
        }

        public async Task DeleteSongAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            if (song == null)
                throw new Exception("Песня не найдена");

            await _songRepository.DeleteAsync(song);
        }
    }
}
