using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels
{
    public class AuthData
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
        public string Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
    }
}
