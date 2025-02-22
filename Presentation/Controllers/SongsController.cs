using Chords.Application.Interfaces;
using Chords.Domain.Entities;
using Chords.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chords.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }

        // GET /api/songs
        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            return Ok(songs);
        }

        // GET /api/songs/{id}
        [HttpGet("{id}")]
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
                Title = model.Title,
                Author = model.Author,
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

            song.Title = model.Title;
            song.Author = model.Author;
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
