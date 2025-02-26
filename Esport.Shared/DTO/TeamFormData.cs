using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esport.Shared.DTO
{
    class TeamFormData
    {
        // Dane gracza przesyłane jako JSON
        [Required]
        public string TeamJson { get; set; } = string.Empty;

        // Plik obrazu (opcjonalnie)
        public IFormFile? Image { get; set; }
    }
}
