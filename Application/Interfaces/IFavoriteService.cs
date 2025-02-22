using Chords.Domain.Entities;

namespace Chords.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(int userId, int songId);
        Task RemoveFavoriteAsync(int userId, int songId);
        Task<IEnumerable<Song>> GetFavoritesAsync(int userId);
    }
}
