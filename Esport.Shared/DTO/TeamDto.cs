using System;
using System.Collections.Generic;

namespace Esport.Shared.DTO
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime FoundedDate { get; set; }
        public string CountryOfOrigin { get; set; } = null!;
        // Kolekcja graczy
        public List<PlayerDto> Players { get; set; } = new();
    }
}
