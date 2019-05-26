using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement
{
    public class InstitutionConUpdate
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Numero { get; set; }
        public string Pagina { get; set; }
        public string CodPostal { get; set; }
        public string Morada { get; set; }
        public string Password { get; set; }
    }
}
