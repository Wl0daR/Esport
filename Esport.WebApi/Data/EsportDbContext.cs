using Microsoft.EntityFrameworkCore;
using Esport.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Esport.WebApi.Data
{
    // Jeśli korzystasz z Identity, pamiętaj, aby dziedziczyć po IdentityDbContext (co zapewnia integrację z Identity)
    public class EsportDbContext : IdentityDbContext
    {
        public EsportDbContext(DbContextOptions<EsportDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Konfiguracja relacji między Team a Tournament (w relacji wiele-do-wielu)
            builder.Entity<Team>()
                .HasMany(t => t.Tournaments)
                .WithMany(t => t.Teams)
                .UsingEntity(j => j.ToTable("TeamTournaments"));
        }
    }
}
