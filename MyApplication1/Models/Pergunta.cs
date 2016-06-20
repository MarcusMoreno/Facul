namespace MyApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pergunta")]
    public partial class Pergunta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pergunta()
        {
            AvaliacaoRespostas = new HashSet<AvaliacaoResposta>();
        }

        [Key]
        public int IdPergunta { get; set; }

        [Required]
        [StringLength(50)]
        public string DescricaoPergunta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliacaoResposta> AvaliacaoRespostas { get; set; }
    }
}
