﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaculdadeSI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReviewEntities : DbContext
    {
        public ReviewEntities()
            : base("name=ReviewEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Avaliacao> Avaliacaos { get; set; }
        public virtual DbSet<AvaliacaoPergunta> AvaliacaoPerguntas { get; set; }
        public virtual DbSet<AvaliacaoResposta> AvaliacaoRespostas { get; set; }
        public virtual DbSet<Perfil> Perfils { get; set; }
        public virtual DbSet<Pergunta> Perguntas { get; set; }
        public virtual DbSet<PerguntaTipoResposta> PerguntaTipoRespostas { get; set; }
        public virtual DbSet<TipoResposta> TipoRespostas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}