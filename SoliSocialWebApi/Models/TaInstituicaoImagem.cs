using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaInstituicaoImagem
    {
        public long Id { get; set; }
        public string InstituicaoId { get; set; }
        public byte[] Image { get; set; }

        public virtual TdInstituicao Instituicao { get; set; }
    }
}
