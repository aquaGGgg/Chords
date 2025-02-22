using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Domain.Interfaces;

namespace Chords.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(string userName, string email, string password)
        {
            // Проверка на существование пользователя
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("Пользователь с таким email уже существует");

            // Хэширование пароля (например, с помощью BCrypt)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);

            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Неверные учетные данные");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
