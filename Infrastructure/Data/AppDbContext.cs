using Microsoft.EntityFrameworkCore;
using Chords.Domain.Entities;

namespace Chords.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<UserSong> UserSongs { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.LyricsWithChords).IsRequired();
            });

            modelBuilder.Entity<UserSong>(entity =>
            {
                entity.HasKey(us => new { us.UserId, us.SongId });
                entity.HasOne(us => us.User)
                      .WithMany(u => u.FavoriteSongs)
                      .HasForeignKey(us => us.UserId);
                entity.HasOne(us => us.Song)
                      .WithMany(s => s.UserSongs)
                      .HasForeignKey(us => us.SongId);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Token).IsRequired();
                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
