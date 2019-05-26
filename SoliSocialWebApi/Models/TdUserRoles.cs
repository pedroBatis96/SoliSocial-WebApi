using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdUserRoles
    {
        public TdUserRoles()
        {
            TaUserRoles = new HashSet<TaUserRoles>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<TaUserRoles> TaUserRoles { get; set; }
    }
}
