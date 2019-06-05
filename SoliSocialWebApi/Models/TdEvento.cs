using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdEvento
    {
        public TdEvento()
        {
            TaEventoImagem = new HashSet<TaEventoImagem>();
            TaParticEvento = new HashSet<TaParticEvento>();
            TdNoticias = new HashSet<TdNoticias>();
        }

        public long Id { get; set; }
        public string InstId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Pagina { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? NumParticipantesMax { get; set; }
        public int? NumStaffMaximo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string CriadoPor { get; set; }

        public virtual TdUsers CriadoPorNavigation { get; set; }
        public virtual TdInstituicao Inst { get; set; }
        public virtual ICollection<TaEventoImagem> TaEventoImagem { get; set; }
        public virtual ICollection<TaParticEvento> TaParticEvento { get; set; }
        public virtual ICollection<TdNoticias> TdNoticias { get; set; }
    }
}
