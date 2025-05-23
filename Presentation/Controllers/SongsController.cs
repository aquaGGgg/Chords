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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            return Ok(songs);
        }

        [HttpGet("authors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthors()
        {
            var songs = await _songService.GetAllSongsAsync();
            var authors = songs.Select(s => s.Author).Distinct();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _songService.GetSongByIdAsync(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
