using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdDocSupp
    {
        public TdDocSupp()
        {
            TaInstDoc = new HashSet<TaInstDoc>();
        }

        public long Id { get; set; }
        public byte[] Doc { get; set; }

        public virtual ICollection<TaInstDoc> TaInstDoc { get; set; }
    }
}
