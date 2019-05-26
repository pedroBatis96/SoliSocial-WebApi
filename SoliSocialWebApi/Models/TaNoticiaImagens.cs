using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaNoticiaImagens
    {
        public long Id { get; set; }
        public long NoticiaId { get; set; }
        public byte[] Image { get; set; }

        public virtual TdNoticias Noticia { get; set; }
    }
}
