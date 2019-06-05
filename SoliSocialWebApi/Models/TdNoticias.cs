using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdNoticias
    {
        public TdNoticias()
        {
            TaNoticiaImagens = new HashSet<TaNoticiaImagens>();
        }

        public long Id { get; set; }
        public string InstId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Pagina { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string CriadoPor { get; set; }
        public long? EventoId { get; set; }

        public virtual TdUsers CriadoPorNavigation { get; set; }
        public virtual TdEvento Evento { get; set; }
        public virtual TdInstituicao Inst { get; set; }
        public virtual ICollection<TaNoticiaImagens> TaNoticiaImagens { get; set; }
    }
}
