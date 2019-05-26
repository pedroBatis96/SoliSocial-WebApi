using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaInstDoc
    {
        public Guid InstId { get; set; }
        public long IdDoc { get; set; }

        public virtual TdDocSupp IdDocNavigation { get; set; }
        public virtual TdInstituicao Inst { get; set; }
    }
}
