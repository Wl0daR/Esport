using Microsoft.AspNetCore.Http;

namespace Esport.Shared.DTO
{
    public class TournamentFormData
    {
        // Dane turnieju przesyłane jako JSON
        public string TournamentJson { get; set; } = string.Empty;

        // Opcjonalny plik obrazu
        public IFormFile? Image { get; set; }
    }
}
