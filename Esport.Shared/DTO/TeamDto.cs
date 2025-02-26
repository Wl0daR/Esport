using System;
using System.Collections.Generic;
using Esport.Shared.Models;

namespace Esport.Shared.DTO
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime FoundedDate { get; set; }
        public string CountryOfOrigin { get; set; } = null!;
        public string? ImagePath { get; set; }
        // Kolekcja graczy
        public List<Player> Players { get; set; } = new();
    }
}
