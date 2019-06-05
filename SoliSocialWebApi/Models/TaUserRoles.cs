using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaUserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual TdUserRoles Role { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
