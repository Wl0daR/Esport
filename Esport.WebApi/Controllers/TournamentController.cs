using Esport.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Esport.WebApi.Data;
using Esport.Shared.DTO;

namespace Esport.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly EsportDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TournamentsController(EsportDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetTournaments()
        {
            var tournaments = await _context.Tournaments.ToListAsync();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournament(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound("Turniej nie został znaleziony.");
            return Ok(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> AddTournament([FromForm] TournamentFormData formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TournamentDto? tournamentDto;
            try
            {
                tournamentDto = System.Text.Json.JsonSerializer.Deserialize<TournamentDto>(
                    formData.TournamentJson,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Nieprawidłowe dane turnieju: " + ex.Message);
            }

            if (tournamentDto == null)
                return BadRequest("Dane turnieju są puste.");

            // Mapowanie z DTO na encję Tournament – mapowanie nowych pól
            var tournament = new Tournament
            {
                Name = tournamentDto.Name,
                Country = tournamentDto.Country,
                Location = tournamentDto.Location,
                StartDate = tournamentDto.StartDate,
                EndDate = tournamentDto.EndDate,
                PrizePool = tournamentDto.PrizePool,
                ImagePath = null // Na początku brak zdjęcia
            };

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();

            // Jeśli przesłano plik obrazu, zapisz go i zaktualizuj ImagePath
            if (formData.Image != null && formData.Image.Length > 0)
            {
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var folderPath = Path.Combine(basePath, "tournamentimages");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{tournament.Id}{Path.GetExtension(formData.Image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Image.CopyToAsync(stream);
                }

                tournament.ImagePath = $"/tournamentimages/{fileName}";
                _context.Tournaments.Update(tournament);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromForm] TournamentFormData formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TournamentDto? updatedDto;
            try
            {
                updatedDto = System.Text.Json.JsonSerializer.Deserialize<TournamentDto>(
                    formData.TournamentJson,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Nieprawidłowe dane turnieju: " + ex.Message);
            }

            if (updatedDto == null)
                return BadRequest("Dane turnieju są puste.");

            if (id != updatedDto.Id)
                return BadRequest("ID w URL musi być zgodne z ID turnieju.");

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound("Turniej nie został znaleziony.");

            // Jeśli przesłano null dla ImagePath i turniej miał już zdjęcie, usuń stary plik
            if (updatedDto.ImagePath == null && !string.IsNullOrEmpty(tournament.ImagePath))
            {
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var filePath = Path.Combine(basePath, tournament.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                tournament.ImagePath = null;
            }

            // Aktualizacja pól turnieju – mapowanie nowych pól
            tournament.Name = updatedDto.Name;
            tournament.Country = updatedDto.Country;
            tournament.Location = updatedDto.Location;
            tournament.StartDate = updatedDto.StartDate;
            tournament.EndDate = updatedDto.EndDate;
            tournament.PrizePool = updatedDto.PrizePool;

            _context.Entry(tournament).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tournaments.Any(t => t.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Zapis nowego pliku obrazu, jeśli został przesłany
            if (formData.Image != null && formData.Image.Length > 0)
            {
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var folderPath = Path.Combine(basePath, "tournamentimages");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{tournament.Id}{Path.GetExtension(formData.Image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Image.CopyToAsync(stream);
                }

                tournament.ImagePath = $"/tournamentimages/{fileName}";
                _context.Tournaments.Update(tournament);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound("Turniej nie został znaleziony.");

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();

            return Ok("Turniej został usunięty.");
        }
    }
}
