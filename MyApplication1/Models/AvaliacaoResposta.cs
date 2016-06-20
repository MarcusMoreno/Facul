namespace MyApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AvaliacaoResposta")]
    public partial class AvaliacaoResposta
    {
        [Key]
        public int IdAvaliacaoResposta { get; set; }

        public int IdAvaliacao { get; set; }

        public int IdUsuario { get; set; }

        public int IdPergunta { get; set; }

        public int IdTipoResposta { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Pergunta Pergunta { get; set; }

        public virtual TipoResposta TipoResposta { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
