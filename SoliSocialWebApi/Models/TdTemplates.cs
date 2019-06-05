using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdTemplates
    {
        public long Id { get; set; }
        public sbyte Evento { get; set; }
        public sbyte Noticia { get; set; }
        public string Pagina { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
