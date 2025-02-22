using Chords.Domain.Entities;

namespace Chords.Domain.Interfaces
{
    public interface ISongRepository
    {
        Task<Song> GetByIdAsync(int id);
        Task<IEnumerable<Song>> GetAllAsync();
        Task AddAsync(Song song);
        Task UpdateAsync(Song song);
        Task DeleteAsync(Song song);
    }
}
