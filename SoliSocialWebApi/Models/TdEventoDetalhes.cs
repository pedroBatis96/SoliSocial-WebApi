using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdEventoDetalhes
    {
        public long NoticiaId { get; set; }
        public string InstId { get; set; }
        public int? NumParticipantesMax { get; set; }
        public int? NumStaffMaximo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public virtual TdNoticias Noticia { get; set; }
    }
}
