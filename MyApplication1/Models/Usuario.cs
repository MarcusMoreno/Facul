namespace MyApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Avaliacaos = new HashSet<Avaliacao>();
            AvaliacaoRespostas = new HashSet<AvaliacaoResposta>();
        }

        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        
        [StringLength(13)]
        public string Telefone { get; set; }

        public int IdPerfil { get; set; }
        
        [Required]
        [StringLength(13)]
        public string Senha { get; set; }

        public bool? UsuarioStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacaos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoResposta> AvaliacaoRespostas { get; set; }

        public virtual Perfil Perfil { get; set; }
    }
}
