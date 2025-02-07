using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Esport.WebApi.Data;            // przestrzeń nazw z EsportDbContext
using Esport.Shared.Models;         // przestrzeń nazw z modelami (Team, Player)

namespace Esport.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly EsportDbContext _context;

        public TeamsController(EsportDbContext context)
        {
            _context = context;
        }

        // GET: api/teams
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            // Używamy Include, aby dołączyć listę graczy dla każdej drużyny
            var teams = await _context.Teams
                                      .Include(t => t.Players)
                                      .ToListAsync();
            return Ok(teams);
        }

        // GET: api/teams/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _context.Teams
                                     .Include(t => t.Players)
                                     .FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        // POST: api/teams
        // Przykładowa metoda dodająca nową drużynę (opcjonalnie)
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }

        // PUT: api/teams/{id}
        // Przykładowa metoda aktualizująca istniejącą drużynę (opcjonalnie)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] Team updatedTeam)
        {
            if (id != updatedTeam.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        // DELETE: api/teams/{id}
        // Przykładowa metoda usuwająca drużynę (opcjonalnie)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Pomocnicza metoda sprawdzająca, czy drużyna istnieje
        private bool TeamExists(int id)
        {
            return _context.Teams.Any(t => t.Id == id);
        }
    }
}
