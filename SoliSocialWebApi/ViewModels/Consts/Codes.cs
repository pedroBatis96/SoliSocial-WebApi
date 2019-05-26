using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.Consts
{
    public class Codes
    {
        public UserRolesCodes UserRoles { get; set; }
        public class UserRolesCodes
        {
            public const string Admin = "adm";

            public const string InstAdmin = "instadm";
        }
    }
}
