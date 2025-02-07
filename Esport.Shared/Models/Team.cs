using System;
using System.Collections.Generic;

namespace Esport.Shared.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Nowe właściwości:
        public DateTime FoundedDate { get; set; }
        public string CountryOfOrigin { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = new List<Player>();

        // Relacja wiele-do-wielu z turniejami
        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
