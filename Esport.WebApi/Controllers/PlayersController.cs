using Esport.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Esport.WebApi.Data;
using Esport.Shared.DTO;

namespace Esport.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly EsportDbContext _context;
        private IWebHostEnvironment _env;

        public PlayersController(EsportDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _context.Players.Include(p => p.Team).ToListAsync();
            return Ok(players);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromForm] PlayerFormData formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Deserializujemy dane gracza z pola PlayerJson
            Esport.Shared.DTO.PlayerDto? updatedDto;
            try
            {
                updatedDto = System.Text.Json.JsonSerializer.Deserialize<Esport.Shared.DTO.PlayerDto>(
                    formData.PlayerJson,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Nieprawidłowe dane gracza: " + ex.Message);
            }

            if (updatedDto == null)
                return BadRequest("Dane gracza są puste.");

            if (id != updatedDto.Id)
                return BadRequest("ID w URL musi być zgodne z ID gracza.");

            // Pobieramy istniejącego gracza z bazy
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound("Gracz nie został znaleziony.");

            if (updatedDto.ImagePath == null && !string.IsNullOrEmpty(player.ImagePath))
            {
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var filePath = Path.Combine(basePath, player.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                player.ImagePath = null;
            }

            // Aktualizacja pól gracza
            player.FirstName = updatedDto.FirstName;
            player.LastName = updatedDto.LastName;
            player.Nickname = updatedDto.Nickname;
            player.DateOfBirth = updatedDto.DateOfBirth;
            player.Country = updatedDto.Country;
            player.Role = updatedDto.Role;
            player.TeamId = updatedDto.TeamId;
            player.ImagePath = updatedDto.ImagePath;

            _context.Entry(player).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Players.Any(p => p.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Jeśli przesłano nowy plik obrazu, zapisz go i zaktualizuj ImagePath
            if (formData.Image != null && formData.Image.Length > 0)
            {
                // Ustal bazową ścieżkę – używamy _env.WebRootPath, jeśli jest ustawione, w przeciwnym razie bieżący katalog
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var folderPath = Path.Combine(basePath, "playerimages");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{player.Id}{Path.GetExtension(formData.Image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Image.CopyToAsync(stream);
                }

                player.ImagePath = $"/playerimages/{fileName}";
                _context.Players.Update(player);
                await _context.SaveChangesAsync();
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

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromForm] PlayerFormData formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            PlayerDto? playerDto;
            try
            {
                playerDto = System.Text.Json.JsonSerializer.Deserialize<PlayerDto>(
                    formData.PlayerJson,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Nieprawidłowe dane gracza: " + ex.Message);
            }

            if (playerDto == null)
                return BadRequest("Dane gracza są puste.");

            if (playerDto.TeamId <= 0)
            {
                return BadRequest("Nie wybrano drużyny.");
            }

            // Mapowanie z DTO na encję Player
            var player = new Player
            {
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                Nickname = playerDto.Nickname,
                DateOfBirth = playerDto.DateOfBirth,
                Country = playerDto.Country,
                Role = playerDto.Role,
                TeamId = playerDto.TeamId,
                ImagePath = null // Na początku brak zdjęcia
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            // Jeśli przesłano plik, zapisz go i zaktualizuj rekord
            if (formData.Image != null && formData.Image.Length > 0)
            {
                var basePath = _env.WebRootPath ?? Directory.GetCurrentDirectory();
                var folderPath = Path.Combine(basePath, "playerimages");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{player.Id}{Path.GetExtension(formData.Image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Image.CopyToAsync(stream);
                }

                player.ImagePath = $"/playerimages/{fileName}";
                _context.Players.Update(player);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound("Gracz nie został znaleziony.");
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok("Gracz został usunięty.");
        }


    }
}
