using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chords.Presentation.Controllers
{
    [ApiController]
    [Authorize]  // Применяется ко всем методам по умолчанию
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }

        // GET /api/songs - Без авторизации
        [HttpGet]
        [AllowAnonymous]  // Отключает авторизацию только для этого метода
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            return Ok(songs);
        }

        // GET /api/songs/authors
        [HttpGet("authors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthors()
        {
            var songs = await _songService.GetAllSongsAsync();
            var authors = songs.Select(s => s.Author).Distinct();
            return Ok(authors);
        }

        // GET /api/songs/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _songService.GetSongByIdAsync(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        // POST /api/songs
        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] SongRequest model)
        {
            var song = new Song
            {
                Author = model.Author,
                Title = model.Title,
                LyricsWithChords = model.LyricsWithChords
            };

            await _songService.CreateSongAsync(song);
            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }

        // PUT /api/songs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongRequest model)
        {
            var song = await _songService.GetSongByIdAsync(id);
            if (song == null) return NotFound();

            song.Author = model.Author;
            song.Title = model.Title;
            song.LyricsWithChords = model.LyricsWithChords;

            await _songService.UpdateSongAsync(song);
            return NoContent();
        }

        // DELETE /api/songs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                await _songService.DeleteSongAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
