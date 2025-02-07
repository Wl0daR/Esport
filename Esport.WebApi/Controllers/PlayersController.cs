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
    }
}
