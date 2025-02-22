using Chords.Domain.Entities;

namespace Chords.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, string userName, string email);
        // Дополнительные методы при необходимости
    }
}
