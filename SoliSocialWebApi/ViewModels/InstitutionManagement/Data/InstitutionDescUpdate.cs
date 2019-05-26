using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement
{
    public class InstitutionDescUpdate
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Acron { get; set; }
        public string Desc { get; set; }
        public string Password { get; set; }
    }
}
