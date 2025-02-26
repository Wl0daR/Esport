using Esport.Shared.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = null!;
    public decimal PrizePool { get; set; }
    public string? ImagePath { get; set; }
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
