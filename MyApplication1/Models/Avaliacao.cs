namespace MyApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Avaliacao")]
    public partial class Avaliacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avaliacao()
        {
            AvaliacaoRespostas = new HashSet<AvaliacaoResposta>();
        }

        [Key]
        public int IdAvaliacao { get; set; }

        [Required]
        [StringLength(50)]
        public string DescricaoAvaliacao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Expiracao { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        public int IdUsuario { get; set; }

        public int IdPerfil { get; set; }

        public bool? AvaliacaoStatus { get; set; }

        public virtual Perfil Perfil { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoResposta> AvaliacaoRespostas { get; set; }
    }
}
