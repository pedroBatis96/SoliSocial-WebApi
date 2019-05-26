using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaEventoImagem
    {
        public long Id { get; set; }
        public long EventoId { get; set; }
        public byte[] Imagem { get; set; }

        public virtual TdEvento Evento { get; set; }
    }
}
