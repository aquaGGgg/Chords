using System.Threading.Tasks;
using Chords.Application.Services;

namespace Chords.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthTokens> RegisterAsync(string userName, string email, string password);
        Task<AuthTokens> LoginAsync(string email, string password);
        Task<AuthTokens> RefreshAsync(string refreshToken);
    }
}
