using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdNoticias
    {
        public TdNoticias()
        {
            TaEventoImagem = new HashSet<TaEventoImagem>();
            TaNoticiaImagens = new HashSet<TaNoticiaImagens>();
            TaParticEvento = new HashSet<TaParticEvento>();
        }

        public long Id { get; set; }
        public string InstId { get; set; }
        public string Nome { get; set; }
        public string Corpo { get; set; }
        public string Resumo { get; set; }
        public string Banner { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string CriadoPor { get; set; }
        public sbyte? Evento { get; set; }

        public virtual TdUsers CriadoPorNavigation { get; set; }
        public virtual TdInstituicao Inst { get; set; }
        public virtual TdEventoDetalhes TdEventoDetalhes { get; set; }
        public virtual ICollection<TaEventoImagem> TaEventoImagem { get; set; }
        public virtual ICollection<TaNoticiaImagens> TaNoticiaImagens { get; set; }
        public virtual ICollection<TaParticEvento> TaParticEvento { get; set; }
    }
}
