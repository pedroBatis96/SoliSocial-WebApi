using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.Consts
{
    public class Defaults
    {
        public UserInstRoles UserRoles { get; set; }
        public class UserInstRoles
        {
            public const string Admin = "Administração";
            public const string Voluntário = "Voluntários";
        }
    }
}
