using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement
{
    public class InstitutionRegister
    {
        public string Name { get; set; }
        public string Acronimo { get; set; }
        public string Descricao { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Telemovel { get; set; }
        public string Pagina { get; set; }
        public string Morada { get; set; }
        public string CodigoPostal { get; set; }
        public string Iban { get; set; }
        public string Nif { get; set; }
    }

    
}
