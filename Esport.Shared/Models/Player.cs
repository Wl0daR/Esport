using System;

namespace Esport.Shared.Models
{
    public class Player
    {
        public int Id { get; set; }

        // Istniejące pole, np. pseudonim (możesz je zachować lub usunąć, jeśli niepotrzebne)
        public string Nickname { get; set; } = null!;

        // Nowe właściwości:
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; } = null!;

        // Relacja z drużyną
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
