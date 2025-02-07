namespace Esport.Shared.DTO
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Country { get; set; } = null!; // np. "Poland", "United States"
        // Możesz dodać dodatkowe właściwości, jeśli potrzebujesz
    }
}
