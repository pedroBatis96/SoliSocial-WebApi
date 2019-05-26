using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaUserRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual TdUserRoles Role { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
