using Esport.Shared.Models;

namespace Esport.Shared.DTO
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Team team {get; set;}
    }
}