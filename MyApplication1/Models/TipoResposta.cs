namespace MyApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoResposta")]
    public partial class TipoResposta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoResposta()
        {
            AvaliacaoRespostas = new HashSet<AvaliacaoResposta>();
        }

        [Key]
        public int IdTipoResposta { get; set; }

        [Required]
        [StringLength(50)]
        public string DescricaoTipoResposta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoResposta> AvaliacaoRespostas { get; set; }
    }
}
