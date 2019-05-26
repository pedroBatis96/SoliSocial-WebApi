using System;

namespace SoliSocialWebApi.ViewModels
{
    public class RegistoModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
    }
}
