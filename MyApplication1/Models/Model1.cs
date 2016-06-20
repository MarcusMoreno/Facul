namespace MyApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=SAS")
        {
        }

        public virtual DbSet<Avaliacao> Avaliacaos { get; set; }
        public virtual DbSet<AvaliacaoResposta> AvaliacaoRespostas { get; set; }
        public virtual DbSet<Perfil> Perfils { get; set; }
        public virtual DbSet<Pergunta> Perguntas { get; set; }
        public virtual DbSet<TipoResposta> TipoRespostas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Avaliacao>()
                .Property(e => e.DescricaoAvaliacao)
                .IsUnicode(false);

            modelBuilder.Entity<Avaliacao>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.AvaliacaoRespostas)
                .WithRequired(e => e.Avaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Perfil>()
                .Property(e => e.DescricaoPerfil)
                .IsUnicode(false);

            modelBuilder.Entity<Perfil>()
                .HasMany(e => e.Avaliacaos)
                .WithRequired(e => e.Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Perfil>()
                .HasMany(e => e.Usuarios)
                .WithRequired(e => e.Perfil)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pergunta>()
                .Property(e => e.DescricaoPergunta)
                .IsUnicode(false);

            modelBuilder.Entity<Pergunta>()
                .HasMany(e => e.AvaliacaoRespostas)
                .WithRequired(e => e.Pergunta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoResposta>()
                .Property(e => e.DescricaoTipoResposta)
                .IsUnicode(false);

            modelBuilder.Entity<TipoResposta>()
                .HasMany(e => e.AvaliacaoRespostas)
                .WithRequired(e => e.TipoResposta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Senha)
                .IsUnicode(false);
               
            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Telefone)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Avaliacaos)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.AvaliacaoRespostas)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);
        }
    }
}
