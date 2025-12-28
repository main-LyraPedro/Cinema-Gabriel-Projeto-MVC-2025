using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CinemaGabriel.Models;

namespace CinemaGabriel.Data
{
    /// <summary>
    /// Contexto da base de dados.
    /// Define as tabelas (DbSets) e configurações de relacionamentos.
    /// BOA PRÁTICA: Configurar explicitamente relacionamentos para evitar ambiguidades.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representam as tabelas na base de dados
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração explícita de relacionamentos usando Fluent API
            
            // Relacionamento 1:N - Filme -> Sessões
            // OnDelete(Cascade): Ao eliminar um Filme, elimina também suas Sessões
            modelBuilder.Entity<Filme>()
                .HasMany(f => f.Sessoes)
                .WithOne(s => s.Filme)
                .HasForeignKey(s => s.FilmeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento 1:N - Sessão -> Reservas
            modelBuilder.Entity<Sessao>()
                .HasMany(s => s.Reservas)
                .WithOne(r => r.Sessao)
                .HasForeignKey(r => r.SessaoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento 1:N - ApplicationUser -> Reservas
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Reservas)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}