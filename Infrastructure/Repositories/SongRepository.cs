using Chords.Domain.Entities;
using Chords.Domain.Interfaces;
using Chords.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Chords.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly AppDbContext _context;
        public SongRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Song song)
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<Song> GetByIdAsync(int id)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }
    }
}
