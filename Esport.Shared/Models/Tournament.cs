using System;
using System.Collections.Generic;

namespace Esport.Shared.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        // Możesz pozostawić istniejącą właściwość Name lub ją rozszerzyć
        public string Name { get; set; } = null!;

        // Nowe właściwości:
        public DateTime TournamentDate { get; set; }
        public string Location { get; set; } = null!;
        public decimal PrizePool { get; set; }
        public string? ImagePath { get; set; }

        // Relacja wiele-do-wielu z drużynami
        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
