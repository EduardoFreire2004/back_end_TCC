using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using API_TCC.Model;

namespace API_TCC.Model
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Semente> Sementes { get; set; }
        public DbSet<Agrotoxico> Agrotoxicos { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Aplicacao> Aplicacoes { get; set; }
        public DbSet<Plantio> Plantios { get; set; }
        public DbSet<Colheita> Colheitas { get; set; }
        public DbSet<Lavoura> Lavouras { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<TipoAgrotoxico> Tipo_Agrotoxicos { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }
        public DbSet<CategoriaInsumo> Categorias_Insumos { get; set; }
        public DbSet<Custo> Custo { get; set; }
        public DbSet<AplicacaoInsumos> Aplicacao_Insumos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para os relacionamentos opcionais em MovimentacaoEstoque
            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Agrotoxico)
                .WithMany()
                .HasForeignKey(m => m.agrotoxicoID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Semente)
                .WithMany()
                .HasForeignKey(m => m.sementeID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Insumo)
                .WithMany()
                .HasForeignKey(m => m.insumoID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para MovimentacaoEstoque - Lavoura (restrict para evitar ciclos)
            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Lavoura)
                .WithMany()
                .HasForeignKey(m => m.lavouraID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração para RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.Usuario)
                .WithMany()
                .HasForeignKey(rt => rt.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurações para evitar ciclos de cascata
            // Usar Restrict para relacionamentos que podem causar problemas
            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.categoriaInsumo)
                .WithMany()
                .HasForeignKey(i => i.categoriaInsumoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.fornecedor)
                .WithMany()
                .HasForeignKey(i => i.fornecedorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Semente>()
                .HasOne(s => s.fornecedor)
                .WithMany()
                .HasForeignKey(s => s.fornecedorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agrotoxico>()
                .HasOne(a => a.fornecedor)
                .WithMany()
                .HasForeignKey(a => a.fornecedorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agrotoxico>()
                .HasOne(a => a.tipo)
                .WithMany()
                .HasForeignKey(a => a.tipoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Plantio>()
                .HasOne(p => p.lavoura)
                .WithMany()
                .HasForeignKey(p => p.lavouraID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Plantio>()
                .HasOne(p => p.semente)
                .WithMany()
                .HasForeignKey(p => p.sementeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Aplicacao>()
                .HasOne(a => a.lavoura)
                .WithMany()
                .HasForeignKey(a => a.lavouraID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Aplicacao>()
                .HasOne(a => a.agrotoxico)
                .WithMany()
                .HasForeignKey(a => a.agrotoxicoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AplicacaoInsumos>()
                .HasOne(ai => ai.lavoura)
                .WithMany()
                .HasForeignKey(ai => ai.lavouraID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AplicacaoInsumos>()
                .HasOne(ai => ai.insumo)
                .WithMany()
                .HasForeignKey(ai => ai.insumoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Colheita>()
                .HasOne(c => c.lavoura)
                .WithMany()
                .HasForeignKey(c => c.lavouraID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Custo>()
                .HasOne(c => c.Lavoura)
                .WithMany(l => l.Custos)
                .HasForeignKey(c => c.LavouraId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Custo>()
                .HasOne(c => c.aplicacao)
                .WithMany()
                .HasForeignKey(c => c.aplicacaoAgrotoxicoID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Custo>()
                .HasOne(c => c.aplicacaoInsumo)
                .WithMany()
                .HasForeignKey(c => c.aplicacaoInsumoID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Custo>()
                .HasOne(c => c.plantio)
                .WithMany()
                .HasForeignKey(c => c.plantioID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Custo>()
                .HasOne(c => c.colheita)
                .WithMany()
                .HasForeignKey(c => c.colheitaID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }

        internal async Task GetEstatisticasColheita(int lavouraId)
        {
            throw new NotImplementedException();
        }
    }
}
