using SoliSocialWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.UserManagement
{
    public class UserInfoSent
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public sbyte EmailConfirmed { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Genero { get; set; }
        public string Imagem { get; set; }
        public string Phonenumber { get; set; }
        public sbyte PhonenumberConfirmed { get; set; }
        public List<tdInsituicaoInfo> InstituicaoList { get; set; }

        public class tdInsituicaoInfo
        {
            public string Id { get; set; }
            public string Acronimo { get; set; }
            public string Logo { get; set; }
        }
    }
}
