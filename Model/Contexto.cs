using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Model
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Semente> Sementes { get; set; }
        public DbSet<Agrotoxico> Agrotoxicos { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Aplicacao> Aplicacoes { get; set; }
        public DbSet<Plantio> Plantios { get; set; }
        public DbSet<Colheita> Colheitas { get; set; }
        public DbSet<Lavoura> Lavouras { get; set; }
        public DbSet<FornecedorAgrotoxico> Fornecedores_Agrotoxico { get; set; }
        public DbSet<FornecedorInsumo> Fornecedores_Insumos { get; set; }
        public DbSet<FornecedorSemente> Fornedores_Sementes { get; set; }
        public DbSet<TipoAgrotoxico> Tipo_Agrotoxicos { get; set; }
        public DbSet<TipoMovimentacao> Tipo_Movimentacoes { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<CategoriaInsumo> Categorias_Insumos { get; set; }

    
    }
}
