using System.ComponentModel.DataAnnotations;

namespace SoliSocialWebApi.ViewModels.UserManagement
{
    public class UserPassUpdate
    {
        [Required]
        public string PasswordOld { get; set; }
        [Required]
        public string PasswordNew { get; set; }
    }
}
