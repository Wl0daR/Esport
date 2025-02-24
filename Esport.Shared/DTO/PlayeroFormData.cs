using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Esport.Shared.DTO
{
    public class PlayerFormData
    {
        // Dane gracza przesyłane jako JSON
        [Required]
        public string PlayerJson { get; set; } = string.Empty;

        // Plik obrazu (opcjonalnie)
        public IFormFile? Image { get; set; }
    }
}
