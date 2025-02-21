using Esport.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Esport.WebApi.Data;

namespace Esport.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly EsportDbContext _context;

        public PlayersController(EsportDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _context.Players.Include(p => p.Team).ToListAsync();
            return Ok(players);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player updatedPlayer)
        {
            if (id != updatedPlayer.Id)
            {
                return BadRequest("ID w URL musi być zgodne z ID gracza.");
            }

            // Ustaw stan encji jako zmodyfikowanej
            _context.Entry(updatedPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Sprawdź, czy gracz o podanym ID istnieje
                if (!_context.Players.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team) // Dołączenie danych drużyny
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

    }
}
