using Chords.Domain.Entities;

namespace Chords.Application.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(int id);
        Task CreateSongAsync(Song song);
        Task UpdateSongAsync(Song song);
        Task DeleteSongAsync(int id);
    }
}
