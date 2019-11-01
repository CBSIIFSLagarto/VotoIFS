using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nota2.Models;

namespace Nota2.Data
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {

        }
        public DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Voto> Votos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Usuario>(usuario =>
            {
                usuario.HasKey(p => p.UseID);
                usuario.HasOne(p => p.TipoUsuario).WithMany(p => p.Usuarios).HasForeignKey(p => p.TipoId);

            });
            builder.Entity<Campanha>(campanha =>
            {
                campanha.HasKey(p => p.CamID);
                campanha.HasOne(p => p.Usuario).WithMany(p => p.Campanhas).HasForeignKey(p => p.UseId);
            });
            builder.Entity<Voto>(voto =>
            {
                voto.HasKey(p => p.VotID);
                voto.HasOne(p => p.Campanha).WithMany(p => p.Votos).HasForeignKey(p => p.CamId);
            });
        }
    }
}
