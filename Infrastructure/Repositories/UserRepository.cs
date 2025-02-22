using Chords.Domain.Entities;
using Chords.Domain.Interfaces;
using Chords.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chords.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.FavoriteSongs)
                    .ThenInclude(fs => fs.Song)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.FavoriteSongs)
                    .ThenInclude(fs => fs.Song)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
