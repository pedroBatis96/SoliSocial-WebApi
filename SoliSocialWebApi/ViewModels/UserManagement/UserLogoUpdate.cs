using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.UserManagement
{
    public class UserImageUpdate
    {
        public string Image { get; set; }

        public string Password { get; set; }
    }
}
