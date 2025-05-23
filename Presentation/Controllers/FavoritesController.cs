using Microsoft.AspNetCore.Mvc;
using Chords.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Chords.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        [HttpPost("add/{songId}")]
        public async Task<IActionResult> AddFavorite(int songId)
        {
            try
            {
                int userId = GetUserId();
                await _favoriteService.AddFavoriteAsync(userId, songId);
                return Ok(new { message = "Песня добавлена в избранное" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("remove/{songId}")]
        public async Task<IActionResult> RemoveFavorite(int songId)
        {
            try
            {
                int userId = GetUserId();
                await _favoriteService.RemoveFavoriteAsync(userId, songId);
                return Ok(new { message = "Песня удалена из избранного" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                int userId = GetUserId();
                var favorites = await _favoriteService.GetFavoritesAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
