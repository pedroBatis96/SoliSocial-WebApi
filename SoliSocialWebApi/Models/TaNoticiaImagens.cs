using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaNoticiaImagens
    {
        public long Id { get; set; }
        public long NoticiaId { get; set; }
        public string Image { get; set; }
        public string Descricao { get; set; }

        public virtual TdNoticias Noticia { get; set; }
    }
}
