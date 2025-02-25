using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Domain.Interfaces;

namespace Chords.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(int id, string userName, string email)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("Пользователь не найден");

            user.UserName = userName;
            user.Email = email;
            await _userRepository.UpdateAsync(user);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}
