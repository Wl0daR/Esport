using System;

namespace Esport.Shared.DTO
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Nowe pola
        public string Country { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Location { get; set; } = string.Empty;
        public decimal PrizePool { get; set; }
        public string? ImagePath { get; set; }
    }
}
