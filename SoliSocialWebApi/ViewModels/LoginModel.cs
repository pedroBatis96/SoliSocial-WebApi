using System.ComponentModel.DataAnnotations;

namespace SoliSocialWebApi.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
