using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Domain.Interfaces;
using Chords.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chords.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, AppDbContext dbContext)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<AuthTokens> RegisterAsync(string userName, string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("Пользователь с таким email уже существует");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            return GenerateTokens(user);
        }

        public async Task<AuthTokens> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Неверные учетные данные");

            return GenerateTokens(user);
        }

        public async Task<AuthTokens> RefreshAsync(string refreshToken)
        {
            var tokenEntity = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null || (tokenEntity.ExpiresAt.HasValue && tokenEntity.ExpiresAt < DateTime.UtcNow))
                throw new SecurityTokenException("Invalid refresh token");

            _dbContext.RefreshTokens.Remove(tokenEntity);
            await _dbContext.SaveChangesAsync();

            return GenerateTokens(tokenEntity.User);
        }

        private AuthTokens GenerateTokens(User user)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _dbContext.RefreshTokens.Add(tokenEntity);
            _dbContext.SaveChanges();

            return new AuthTokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenLifetimeMinutes"] ?? "30")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class AuthTokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
