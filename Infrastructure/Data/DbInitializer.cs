using Chords.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chords.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            if (!context.Users.Any(u => u.Email == "admin@chords.local"))
            {
                var admin = new User
                {
                    UserName = "Admin",
                    Email = "admin@chords.local",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
