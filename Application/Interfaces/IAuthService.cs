namespace Chords.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string userName, string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
