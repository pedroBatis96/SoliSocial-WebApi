using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaUserInstituicaoFav
    {
        public Guid UserId { get; set; }
        public Guid InstituicaoId { get; set; }

        public virtual TdInstituicao Instituicao { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
