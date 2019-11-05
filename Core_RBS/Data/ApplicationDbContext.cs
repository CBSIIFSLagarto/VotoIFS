using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core_RBS.Models;


namespace Core_RBS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Voto> Votos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Campanha>(campanha =>
            {
                campanha.HasKey(p => p.CamID);                
            });

            builder.Entity<Voto>(voto =>
            {
                voto.HasKey(p => p.VotID);
                voto.HasOne(p => p.Campanha).WithMany(p => p.Votos).HasForeignKey(p => p.CamId);
            });
        }

    }
}
