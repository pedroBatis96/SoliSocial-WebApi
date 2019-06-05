using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaUserInstituicaoBlock
    {
        public string UserId { get; set; }
        public string InstituicaoId { get; set; }

        public virtual TdInstituicao Instituicao { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
