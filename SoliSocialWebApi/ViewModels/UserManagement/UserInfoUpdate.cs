using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.UserManagement
{
    public class UserInfoUpdate
    {
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Bio { get; set; }
        public string Genero { get; set; }
        public string Password { get; set; }
    }
}
