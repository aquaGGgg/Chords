using Microsoft.EntityFrameworkCore;
using Chords.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Chords.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<UserSong> UserSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Конфигурация Song
            modelBuilder.Entity<Song>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.LyricsWithChords).IsRequired();
            });

            // Конфигурация связи многие-ко-многим через UserSong
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
        }
    }
}
