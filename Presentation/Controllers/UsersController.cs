using Chords.Application.Interfaces;
using Chords.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chords.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFavoriteService _favoriteService;

        public UsersController(IUserService userService, IFavoriteService favoriteService)
        {
            _userService = userService;
            _favoriteService = favoriteService;
        }

        // GET /api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            // ДОБАВИТЬ МЕТОД РЕПОЗИТОРИЯ
            return Ok();
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // PUT /api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest model)
        {
            try
            {
                await _userService.UpdateUserAsync(id, model.UserName, model.Email);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST /api/users/{userId}/favorites
        [HttpPost("{userId}/favorites")]
        public async Task<IActionResult> AddFavorite(int userId, [FromBody] FavoriteRequest model)
        {
            try
            {
                await _favoriteService.AddFavoriteAsync(userId, model.SongId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Новый endpoint для добавления избранного по email
        // POST /api/users/favorites/by-email
        [HttpPost("favorites/by-email")]
        public async Task<IActionResult> AddFavoriteByEmail([FromBody] FavoriteByEmailRequest model)
        {
            try
            {
                // Предполагаем, что у вас реализован метод поиска пользователя по email
                var user = await _userService.GetUserByEmailAsync(model.Email);
                if (user == null)
                    return NotFound(new { message = "Пользователь не найден" });

                await _favoriteService.AddFavoriteAsync(user.Id, model.SongId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("favorites/by-email")]
        public async Task<IActionResult> GetFavoritesByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                    return NotFound(new { message = "Пользователь не найден" });

                var favorites = await _favoriteService.GetFavoritesAsync(user.Id);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/users/{userId}/favorites/{songId}
        [HttpDelete("{userId}/favorites/{songId}")]
        public async Task<IActionResult> RemoveFavorite(int userId, int songId)
        {
            try
            {
                await _favoriteService.RemoveFavoriteAsync(userId, songId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/users/{userId}/favorites
        [HttpGet("{userId}/favorites")]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
